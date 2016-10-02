using System;
using Skimia.Extensions.Discovery.Attributes.Abstractions;

namespace Skimia.Extensions.Discovery.Attributes.Abstractions
{
    public interface IAttributeDiscoveredHandler
    {
        Action<object, IHandler, HandlerAttribute> Action { get; }

        object Container { get; }

        Type ContainerType { get; }

        AttributeDiscoveredHandlerAttribute HandlerAttribute { get; }

        Type HandlerAttributeType { get; }
    }
}