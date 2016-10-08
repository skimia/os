using System;
using System.Reflection;

namespace Skimia.Extensions.Discovery.Attributes.Abstractions
{
    public interface IMethodHandler
    {
        MethodHandlerAttribute HandlerAttribute { get; }

        MethodInfo MethodInfo { get; }

        object Container { get; }

        Type ContainerType { get; }

        Delegate CreateDelegate(params Type[] delegParams);
    }
}
