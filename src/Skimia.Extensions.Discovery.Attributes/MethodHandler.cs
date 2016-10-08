using System;
using System.Reflection;
using Skimia.Extensions.Discovery.Attributes.Abstractions;
using Skimia.Extensions.Reflection;

namespace Skimia.Extensions.Discovery.Attributes
{
    public class MethodHandler : IMethodHandler
    {
        public MethodHandler(MethodHandlerAttribute handlerAttribute, MethodInfo methodInfo, Type containerType, object container = null)
        {
            HandlerAttribute = handlerAttribute;
            MethodInfo = methodInfo;
            ContainerType = containerType;
            Container = container;
        }

        public MethodHandlerAttribute HandlerAttribute
        {
            get;
            private set;
        }

        public MethodInfo MethodInfo
        {
            get;
            private set;
        }

        public Type ContainerType
        {
            get;
            private set;
        }

        public object Container
        {
            get;
            private set;
        }

        public Delegate CreateDelegate(params Type[] delegParams)
        {
            return MethodInfo.CreateParamsDelegate(delegParams);
        }
    }
}
