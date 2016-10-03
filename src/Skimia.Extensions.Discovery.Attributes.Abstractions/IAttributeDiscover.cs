using System;
using System.Collections.Generic;
using System.Reflection;

namespace Skimia.Extensions.Discovery.Attributes.Abstractions
{
    public interface IAttributeDiscover
    {
        IEnumerable<IMethodAttributeDiscoveredHandler> GetMethodAttributeDiscoveredHandlers(Type handlerAttributeType);

        IEnumerable<IMethodHandler> GetMethodHandlers(Type handlerAttributeType);
        IEnumerable<IClassHandler> GetClassHandlers(Type handlerAttributeType);

        void RegisterAssemblies(IEnumerable<Assembly> assemblies);

        void RegisterAssembly(Assembly assembly);

        void RegisterInstanceContainer(object container);
    }
}
