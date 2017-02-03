using System;
using Skimia.Extensions.Discovery.Attributes.Abstractions;

namespace Skimia.Extensions.Discovery.Attributes
{
    public class ClassHandler : IClassHandler
    {
        public ClassHandler(ClassHandlerAttribute handlerAttribute, Type containerType, object container)
        {
            HandlerAttribute = handlerAttribute;
            ContainerType = containerType;
            Container = container;
        }

        public ClassHandlerAttribute HandlerAttribute
        {
            get;
            private set;
        }

        public Type ContainerType
        {
            get;
            private set;
        }

        public object Container
        {
            get;
            private set;
        }
    }
}
