using System;
using System.Collections.Generic;
using ExtCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Skimia.Extensions.Discovery.Attributes.Abstractions;
using Skimia.Extensions.GraphQl.Schema;

namespace Skimia.Extensions.GraphQl
{
    public class GraphQl : ExtensionBase
    {
        public override string Name => "Skimia.Plugins.Auth";

        public override IEnumerable<KeyValuePair<int, Action<IServiceCollection>>> ConfigureServicesActionsByPriorities
        {
            get
            {
                return new Dictionary<int, Action<IServiceCollection>>()
                {
                    [1000] = collection =>
                    {
                        collection.AddSingleton<RootSchema>();
                        collection.AddSingleton<Query>();
                    }
                };
            }
        }
    }
}
