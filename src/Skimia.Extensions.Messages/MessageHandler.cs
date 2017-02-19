using System;
using Skimia.Extensions.Discovery.Attributes.Abstractions;
using Skimia.Extensions.Messages.Abstractions;
using Skimia.Extensions.Reflection;

namespace Skimia.Extensions.Messages
{
    public class MessageHandler : IMessageHandler
    {
        public MessageHandler(IMethodHandler methodHandler)
        {
            Container = methodHandler.Container;
            ContainerType = methodHandler.ContainerType;
            Attribute = (MessageHandlerAttribute)methodHandler.HandlerAttribute;
            Action = CreateAction(methodHandler);
            MessageType = Attribute.MessageType;
            TokenType = methodHandler.MethodInfo.GetParameters()[0].ParameterType;
        }

        public MessageHandler(object container, Type containerType, Type messageType, MessageHandlerAttribute handlerAttribute, Action<object, object, IMessage> action, Type tokenType)
        {
            Container = container;
            ContainerType = containerType;
            MessageType = messageType;
            Attribute = handlerAttribute;
            Action = action;
            TokenType = tokenType;
        }

        public object Container
        {
            get;
        }

        public Type ContainerType
        {
            get;
        }

        public Type MessageType
        {
            get;
        }

        public MessageHandlerAttribute Attribute
        {
            get;
        }

        public Action<object, object, IMessage> Action
        {
            get;
        }

        public Type TokenType
        {
            get;
        }

        private Action<object, object, IMessage> CreateAction(IMethodHandler methodHandler)
        {
            var parameters = methodHandler.MethodInfo.GetParameters();
            if (parameters.Length != 2
                || (parameters[0].ParameterType != typeof(object))
                || (!parameters[1].ParameterType.HasInterface(typeof(IMessage)) && parameters[1].ParameterType != typeof(IMessage))
                )
            {
                throw new ArgumentException(string.Format("Message handler {0} has incorrect parameters. Right definition is Handler(object, IMessage)", methodHandler.MethodInfo));
            }

            Action<object, object, IMessage> handlerDelegate;
            try
            {
                handlerDelegate = (Action<object, object, IMessage>)methodHandler.CreateDelegate(typeof(object), typeof(IMessage));
            }
            catch (Exception)
            {
                throw new ArgumentException(string.Format("Message handler {0} has incorrect parameters. Right definition is Handler(object, IMessage)", methodHandler.MethodInfo));
            }

            return handlerDelegate;
        }
    }
}
