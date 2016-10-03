using Skimia.Extensions.Discovery.Attributes.Abstractions;
using System;

namespace Skimia.Extensions.Discovery.Attributes
{
    public class MethodAttributeDiscoveredHandler : IMethodAttributeDiscoveredHandler
    {
        public MethodAttributeDiscoveredHandler(Type handlerAttributeType, object container, Type containerType, MethodAttributeDiscoveredHandlerAttribute handlerAttribute, Action<object, IMethodHandler, MethodHandlerAttribute> action)
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

        public Action<object, IMethodHandler, MethodHandlerAttribute> Action
        {
            get;
            private set;
        }

        public MethodAttributeDiscoveredHandlerAttribute HandlerAttribute
        {
            get;
            private set;
        }
    }
}
