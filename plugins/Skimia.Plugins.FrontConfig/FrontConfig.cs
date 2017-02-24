using System;
using System.Collections.Generic;
using ExtCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Skimia.Plugins.FrontConfig.Configuration;
using Skimia.Plugins.FrontConfig.Schema.Result;
using Skimia.Plugins.FrontConfig.Service;

namespace Skimia.Plugins.FrontConfig
{
    public class FrontConfig : ExtensionBase
    {
        public override string Name => "Skimia.Plugins.FrontConfig";
        
        public override IEnumerable<KeyValuePair<int, Action<IServiceCollection>>> ConfigureServicesActionsByPriorities
        {
            get
            {
                return new Dictionary<int, Action<IServiceCollection>>()
                {
                    [1000] = services =>
                    {
                        services.AddSingleton<FrontConfigService>();
                        services.AddSingleton<Schema.FrontConfig>();
                        services.AddSingleton<FrontConfiguration>();
                        services.AddSingleton<FrontState>();
                        services.AddSingleton<FrontView>();

                        services.Configure<FrontConfigOptions>(options => configurationRoot.GetSection("Plugins:FrontConfig").Bind(options));
                    }
                };
            }
        }
    }
}
