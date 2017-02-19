using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Skimia.Extensions.Discovery.Attributes.Abstractions;
using Skimia.Extensions.Messages.Abstractions;
using System.Linq;
using System.Reflection;

namespace Skimia.Extensions.Messages
{
    public class MessageDispatcher : IMessageDispatcher
    {
        private readonly SortedDictionary<MessagePriority, Queue<Tuple<IMessage, object>>> _messagesToDispatch = new SortedDictionary<MessagePriority, Queue<Tuple<IMessage, object>>>();
        private readonly Dictionary<Type, List<IMessageHandler>> _messageHandlers = new Dictionary<Type, List<IMessageHandler>>();
        private int _currentThreadId;
        private object _currentProcessor;

        public object CurrentProcessor => _currentProcessor;

        public int CurrentThreadId => _currentThreadId;


        private readonly ManualResetEventSlim _resumeEvent = new ManualResetEventSlim(true);
        private readonly ManualResetEventSlim _messageEnqueuedEvent = new ManualResetEventSlim(false);

        private bool _stopped;
        private bool _dispatching;

        public bool Stopped => _stopped;

        public event Action<IMessageDispatcher, IMessage> MessageDispatched;

        private void OnMessageDispatched(IMessage message)
        {
            MessageDispatched?.Invoke(this, message);
        }

        public MessageDispatcher()
        {
            foreach (var value in Enum.GetValues(typeof(MessagePriority)))
            {
                _messagesToDispatch.Add((MessagePriority)value, new Queue<Tuple<IMessage, object>>());
            }
        }

        public IEnumerable<IMessageHandler> GetHandlers(Type messageType, object token = null)
        {

                List<IMessageHandler> handlersList;
                if (_messageHandlers.TryGetValue(messageType, out handlersList))
                    foreach (var handler in handlersList)
                        if (token == null || handler.TokenType.IsInstanceOfType(token))
                            yield return handler;


            // note : disabled yet.

            // recursivity to handle message from base class
            //if (messageType.BaseType != null && messageType.BaseType.IsSubclassOf(typeof(Message)))
            //    foreach (var handler in GetHandlers(messageType.BaseType, token))
            //    {
            //        if (handler.Attribute.HandleChildMessages)
            //            yield return handler;
            //    }
        }

        public void Enqueue(IMessage message, bool executeIfCan = true)
        {
            Enqueue(message, null, executeIfCan);
        }

        public virtual void Enqueue(IMessage message, object token, bool executeIfCan = true)
        {
            if (executeIfCan && IsInDispatchingContext())
            {
                Dispatch(message, token);
            }
            else
            {
                lock (_messageEnqueuedEvent)
                {
                    _messagesToDispatch[message.Priority].Enqueue(Tuple.Create(message, token));

                    if (!_dispatching)
                        _messageEnqueuedEvent.Set();
                }
            }
        }

        public bool IsInDispatchingContext()
        {
            return Thread.CurrentThread.ManagedThreadId == _currentThreadId &&
            _currentProcessor != null;
        }

        public void ProcessDispatching(object processor)
        {
            if (_stopped)
                return;

            if (Interlocked.CompareExchange(ref _currentThreadId, Thread.CurrentThread.ManagedThreadId, 0) == 0)
            {
                _currentProcessor = processor;
                _dispatching = true;

                var copy = _messagesToDispatch.ToArray();
                foreach (var keyPair in copy)
                {
                    if (_stopped)
                        break;

                    while (keyPair.Value.Count != 0)
                    {
                        if (_stopped)
                            break;

                        var message = keyPair.Value.Dequeue();

                        if (message != null)
                            Dispatch(message.Item1, message.Item2);
                    }
                }

                _currentProcessor = null;
                _dispatching = false;
                Interlocked.Exchange(ref _currentThreadId, 0);
            }

            lock (_messagesToDispatch)
            {
                if (_messagesToDispatch.Sum(x => x.Value.Count) > 0)
                    _messageEnqueuedEvent.Set();
                else
                    _messageEnqueuedEvent.Reset();
            }
        }

        protected virtual void Dispatch(IMessage message, object token)
        {
            try
            {
                foreach (var handler in GetHandlers(message.GetType(), token).ToArray()) // have to transform it into a collection if we want to add/remove handler
                {
                    handler.Action(handler.Container, token, message);

                    if (message.Canceled)
                        break;
                }

                message.OnDispatched();
                OnMessageDispatched(message);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Cannot dispatch {0}", message), ex);
            }
        }

        /// <summary>
        /// Block the current thread until a message is enqueued
        /// </summary>
        public void Wait()
        {
            if (_stopped)
                _resumeEvent.Wait();

            if (_messagesToDispatch.Sum(x => x.Value.Count) > 0)
                return;

            _messageEnqueuedEvent.Wait();
        }

        public void Resume()
        {
            if (!_stopped)
                return;

            _stopped = false;
            _resumeEvent.Set();
        }

        public void Stop()
        {
            if (_stopped)
                return;

            _stopped = true;
            _resumeEvent.Reset();
        }

        public void Dispose()
        {
            Stop();

            foreach (var messages in _messagesToDispatch)
            {
                messages.Value.Clear();
            }
        }

        private Stopwatch _spy;

        /// <summary>
        /// Says how many milliseconds elapsed since last message. 
        /// </summary>
        public long DelayFromLastMessage
        {
            get
            {
                if (_spy == null) _spy = Stopwatch.StartNew(); return _spy.ElapsedMilliseconds;
            }
        }

        /// <summary>
        /// Reset timer for last received message
        /// </summary>
        protected void ActivityUpdate()
        {
            if (_spy == null)
                _spy = Stopwatch.StartNew();
            else
                _spy.Restart();
        }

        [MethodAttributeDiscoveredHandler(typeof(MessageHandlerAttribute))]
        public void RegisterTestMethodHandler(IMethodHandler handler, MessageHandlerAttribute handlerAttribute)
        {
            var messageHandler = new MessageHandler(handler);

            if (!_messageHandlers.ContainsKey(handlerAttribute.MessageType))
                _messageHandlers.Add(handlerAttribute.MessageType, new List<IMessageHandler>());

            _messageHandlers[handlerAttribute.MessageType].Add(messageHandler);
        }
    }
}
