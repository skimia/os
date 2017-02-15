using System;
using System.Collections.Generic;
using System.IO;
using ExtCore.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Skimia.Extensions.Discovery.Attributes.Abstractions;
using Skimia.Extensions.GraphQl.Schema;

namespace Skimia.Extensions.GraphQl
{
    public class GraphQl : ExtensionBase
    {
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
                    }
                };
            }
        }
    }
}
