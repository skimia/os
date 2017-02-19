using System;

namespace Skimia.Extensions.Discovery.Attributes.Abstractions
{
    public interface IClassAttributeDiscoveredHandler
    {
        Action<object, IClassHandler, ClassHandlerAttribute> Action { get; }

        object Container { get; }

        Type ContainerType { get; }

        ClassAttributeDiscoveredHandlerAttribute HandlerAttribute { get; }

        Type HandlerAttributeType { get; }
    }
}