using Skimia.Extensions.Discovery.Attributes.Abstractions;

namespace Skimia.Extensions.Discovery.Attributes
{
    public class MethodHandler : IMethodHandler
    {
        public MethodHandler(MethodHandlerAttribute handlerAttribute)
        {
            HandlerAttribute = handlerAttribute;
        }

        public MethodHandlerAttribute HandlerAttribute
        {
            get;
            private set;
        }
    }
}
