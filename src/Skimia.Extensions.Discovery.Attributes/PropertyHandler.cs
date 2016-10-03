using Skimia.Extensions.Discovery.Attributes.Abstractions;

namespace Skimia.Extensions.Discovery.Attributes
{
    public class PropertyHandler : IPropertyHandler
    {
        public PropertyHandler(PropertyHandlerAttribute handlerAttribute)
        {
            HandlerAttribute = handlerAttribute;
        }

        public PropertyHandlerAttribute HandlerAttribute
        {
            get;
            private set;
        }
    }
}
