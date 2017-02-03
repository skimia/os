using System;

namespace Skimia.Extensions.Discovery.Attributes.Abstractions
{
    public interface IClassHandler
    {
        ClassHandlerAttribute HandlerAttribute
        {
            get;
        }

        Type ContainerType
        {
            get;
        }

        object Container
        {
            get;
        }
    }
}
