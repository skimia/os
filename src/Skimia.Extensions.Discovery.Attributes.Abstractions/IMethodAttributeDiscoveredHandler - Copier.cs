using System;

namespace Skimia.Extensions.Discovery.Attributes.Abstractions
{
    public interface IMethodAttributeDiscoveredHandler
    {
        Action<object, IMethodHandler, MethodHandlerAttribute> Action { get; }

        object Container { get; }

        Type ContainerType { get; }

        MethodAttributeDiscoveredHandlerAttribute HandlerAttribute { get; }

        Type HandlerAttributeType { get; }
    }
}