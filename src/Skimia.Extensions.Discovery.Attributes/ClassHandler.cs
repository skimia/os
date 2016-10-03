using Skimia.Extensions.Discovery.Attributes.Abstractions;

namespace Skimia.Extensions.Discovery.Attributes
{
    public class ClassHandler : IClassHandler
    {
        public ClassHandler(ClassHandlerAttribute handlerAttribute)
        {
            HandlerAttribute = handlerAttribute;
        }

        public ClassHandlerAttribute HandlerAttribute
        {
            get;
            private set;
        }
    }
}
