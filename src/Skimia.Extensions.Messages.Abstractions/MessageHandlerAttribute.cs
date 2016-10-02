using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
