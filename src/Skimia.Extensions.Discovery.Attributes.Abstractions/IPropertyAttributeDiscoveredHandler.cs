using System;

namespace Skimia.Extensions.Discovery.Attributes.Abstractions
{
    public interface IPropertyAttributeDiscoveredHandler
    {
        Action<object, IPropertyHandler, PropertyHandlerAttribute> Action { get; }

        object Container { get; }

        Type ContainerType { get; }

        PropertyAttributeDiscoveredHandlerAttribute HandlerAttribute { get; }

        Type HandlerAttributeType { get; }
    }
}