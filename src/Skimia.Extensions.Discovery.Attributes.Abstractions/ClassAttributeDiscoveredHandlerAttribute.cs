using System;

namespace Skimia.Extensions.Discovery.Attributes.Abstractions
{
    public class ClassAttributeDiscoveredHandlerAttribute : AbstractAttributeDiscoveredHandlerAttribute
    {
        public ClassAttributeDiscoveredHandlerAttribute(Type type) : base(type)
        {
        }
    }
}
