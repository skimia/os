using System;
using System.Collections.Generic;
using ExtCore.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Glimpse;
using Microsoft.AspNetCore.Hosting;

namespace Skimia.Plugins.Glimpse
{
    public class Glimpse : ExtensionBase
    {
        public override string Name => "Skimia.Plugins.Glimpse";
        
        public override IEnumerable<KeyValuePair<int, Action<IServiceCollection>>> ConfigureServicesActionsByPriorities
        {
            get
            {
                return new Dictionary<int, Action<IServiceCollection>>()
                {
                    [1000] = services =>
                    {
                        if (serviceProvider.GetService<IHostingEnvironment>().IsDevelopment())
                        {
                            services.AddGlimpse();
                        } 
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
                    [1000] = app =>
                    {
                        if (serviceProvider.GetService<IHostingEnvironment>().IsDevelopment())
                        {
                            app.UseGlimpse();
                        }
                    }
                };
            }
        }
    }
}
