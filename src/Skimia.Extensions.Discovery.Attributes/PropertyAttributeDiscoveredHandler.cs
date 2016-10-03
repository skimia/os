using Skimia.Extensions.Discovery.Attributes.Abstractions;
using System;

namespace Skimia.Extensions.Discovery.Attributes
{
    public class PropertyAttributeDiscoveredHandler : IPropertyAttributeDiscoveredHandler
    {
        public PropertyAttributeDiscoveredHandler(Type handlerAttributeType, object container, Type containerType, PropertyAttributeDiscoveredHandlerAttribute handlerAttribute, Action<object, IPropertyHandler, PropertyHandlerAttribute> action)
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

        public Action<object, IPropertyHandler, PropertyHandlerAttribute> Action
        {
            get;
            private set;
        }

        public PropertyAttributeDiscoveredHandlerAttribute HandlerAttribute
        {
            get;
            private set;
        }
    }
}
