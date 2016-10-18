using System;
using System.Threading;
using Skimia.Extensions.Messages.Abstractions;

namespace Skimia.Extensions.Messages
{
    public class Message : IMessage
    {
        public event Action<IMessage> Dispatched;
        private bool _dispatched;

        public Message()
        {
        }

        public bool Canceled
        {
            get;
            set;
        }

        public bool IsDispatched
        {
            get { return _dispatched; }
        }

        public MessagePriority Priority
        {
            get;
            private set;
        }

        public void BlockProgression()
        {
            Canceled = true;
        }

        public void OnDispatched()
        {
            if (_dispatched)
                return;

            lock (this)
            {
                Monitor.PulseAll(this);
            }

            _dispatched = true;

            var evnt = Dispatched;
            if (evnt != null)
                evnt(this);
        }

        public bool Wait()
        {
            return Wait(TimeSpan.Zero); ;
        }

        public bool Wait(TimeSpan timeout)
        {
            if (_dispatched)
                return false;

            lock (this)
            {
                if (timeout > TimeSpan.Zero)
                    Monitor.Wait(this, timeout);
                else
                    Monitor.Wait(this);
            }

            return true;
        }
    }
}
