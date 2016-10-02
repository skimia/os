using Skimia.Extensions.Discovery.Attributes.Abstractions;
using System;

namespace Skimia.Extensions.Discovery.Attributes
{
    public class AttributeDiscoveredHandler : IAttributeDiscoveredHandler
    {
        public AttributeDiscoveredHandler(Type handlerAttributeType, object container, Type containerType, AttributeDiscoveredHandlerAttribute handlerAttribute, Action<object, IHandler, HandlerAttribute> action)
        {
            HandlerAttributeType = handlerAttributeType;
            Container = container;
            ContainerType = containerType;
            HandlerAttribute = handlerAttribute;
            Action = action;
        }

        public object Container
        {
            get;
            private set;
        }

        public Type ContainerType
        {
            get;
            private set;
        }

        public Type HandlerAttributeType
        {
            get;
            private set;
        }

        public Action<object, IHandler, HandlerAttribute> Action
        {
            get;
            private set;
        }

        public AttributeDiscoveredHandlerAttribute HandlerAttribute
        {
            get;
            private set;
        }
    }
}
