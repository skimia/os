using System;
using Skimia.Extensions.Discovery.Attributes.Abstractions;

namespace Skimia.Extensions.Messages.Abstractions
{
    public class MessageHandlerAttribute : MethodHandlerAttribute
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
