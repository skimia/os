using System;

namespace Skimia.Extensions.Discovery.Attributes.Abstractions
{
    public class MethodAttributeDiscoveredHandlerAttribute : AbstractAttributeDiscoveredHandlerAttribute
    {
        public MethodAttributeDiscoveredHandlerAttribute(Type type) : base(type)
        {
        }
    }
}
