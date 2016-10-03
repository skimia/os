using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skimia.Extensions.Discovery.Attributes.Abstractions
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AbstractAttributeDiscoveredHandlerAttribute : Attribute
    {
        public AbstractAttributeDiscoveredHandlerAttribute(Type type)
        {
            HandlerAttributeType = type;
            HandleAlreadyDiscoveredAttributes = true;
        }

        public Type HandlerAttributeType
        {
            get;
            private set;
        }

        public bool HandleAlreadyDiscoveredAttributes
        {
            get;
            set;
        }
    }
}
