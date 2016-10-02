using System;
using System.Collections.Generic;
using System.Reflection;

namespace Skimia.Extensions.Discovery.Attributes.Abstractions
{
    public interface IAttributeDiscover
    {
        IEnumerable<IAttributeDiscoveredHandler> GetAttributeDiscoveredHandlers(Type handlerAttributeType);
        IEnumerable<IHandler> GetHandlers(Type handlerAttributeType);

        void RegisterAssemblies(IEnumerable<Assembly> assemblies);

        void RegisterAssembly(Assembly assembly);

        void RegisterInstanceContainer(object container);
    }
}
