using Skimia.Extensions.Discovery.Attributes.Abstractions;

namespace Skimia.Extensions.Discovery.Attributes
{
    public class Handler : IHandler
    {
        public Handler(HandlerAttribute handlerAttribute)
        {
            HandlerAttribute = handlerAttribute;
        }

        public HandlerAttribute HandlerAttribute
        {
            get;
            private set;
        }
    }
}
