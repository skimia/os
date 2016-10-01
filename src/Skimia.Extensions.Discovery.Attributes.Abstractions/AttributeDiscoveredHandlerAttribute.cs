using System;

namespace Skimia.Extensions.Discovery.Attributes.Abstractions
{
    public class AttributeDiscoveredHandlerAttribute : Attribute
    {
        public AttributeDiscoveredHandlerAttribute(Type type)
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
