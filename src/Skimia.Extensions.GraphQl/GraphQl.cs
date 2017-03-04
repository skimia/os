using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ExtCore.Infrastructure;
using Skimia.Extensions.GraphQl.Schema;

namespace Skimia.Extensions.GraphQl
{
    public class GraphQl : ExtensionBase
    {
        public static IServiceProvider ServiceProvider;

        public override string Name => "Skimia.Extensions.GraphQl";

        public override IEnumerable<KeyValuePair<int, Action<IServiceCollection>>> ConfigureServicesActionsByPriorities
        {
            get
            {
                return new Dictionary<int, Action<IServiceCollection>>()
                {
                    [9000] = collection =>
                    {
                        collection.AddSingleton<RootSchema>();
                        collection.AddSingleton<Query>();
                        collection.AddSingleton<Mutation>();
                        serviceProvider.GetService<RootSchema>();
                    }
                };
            }
        }

        public override IEnumerable<KeyValuePair<int, Action<IApplicationBuilder>>> ConfigureActionsByPriorities
        {
            get
            {

                return new Dictionary<int, Action<IApplicationBuilder>>()
                {
                    [2000] = app =>
                    {
                        app.UseMvc();
                        ServiceProvider = app.ApplicationServices;
                    }
                };
            }
        }
    }
}
