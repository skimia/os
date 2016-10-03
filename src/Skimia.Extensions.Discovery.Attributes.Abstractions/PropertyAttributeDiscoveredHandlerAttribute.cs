using System;

namespace Skimia.Extensions.Discovery.Attributes.Abstractions
{
    public class PropertyAttributeDiscoveredHandlerAttribute : AbstractAttributeDiscoveredHandlerAttribute
    {
        public PropertyAttributeDiscoveredHandlerAttribute(Type type) : base(type)
        {
        }
    }
}
