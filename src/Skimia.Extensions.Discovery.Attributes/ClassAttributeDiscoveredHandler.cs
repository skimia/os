using Skimia.Extensions.Discovery.Attributes.Abstractions;
using System;

namespace Skimia.Extensions.Discovery.Attributes
{
    public class ClassAttributeDiscoveredHandler : IClassAttributeDiscoveredHandler
    {
        public ClassAttributeDiscoveredHandler(Type handlerAttributeType, object container, Type containerType, ClassAttributeDiscoveredHandlerAttribute handlerAttribute, Action<object, IClassHandler, ClassHandlerAttribute> action)
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

        public Action<object, IClassHandler, ClassHandlerAttribute> Action
        {
            get;
            private set;
        }

        public ClassAttributeDiscoveredHandlerAttribute HandlerAttribute
        {
            get;
            private set;
        }
    }
}
