using System;

namespace Skimia.Extensions.Messages.Abstractions
{
    public class MessageHandlerAttribute : Attribute
    {
        public MessageHandlerAttribute(Type messageType)
        {
            MessageType = messageType;
        }

        public Type MessageType
        {
            get;
            private set;
        }
    }
}
