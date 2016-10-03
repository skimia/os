using System;
using ExtCore.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Skimia.Extensions.Discovery.Attributes;
using Skimia.Extensions.Discovery.Attributes.Abstractions;

namespace SkimiaOS.ApiHost
{
    public class Startup : ExtCore.WebApplication.Startup
    {
        public Startup(IHostingEnvironment env, IServiceProvider serviceProvider) 
            : base(serviceProvider)
        {
            
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("config.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"config.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            configurationRoot = builder.Build();

            this.serviceProvider.GetService<ILoggerFactory>().AddConsole(configurationRoot.GetSection("Logging"));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public override void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddDiscovery();

            base.ConfigureServices(services);

            LoadExtensionInDiscover();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public override void Configure(IApplicationBuilder app)
        {
            app.UseMvc();

            base.Configure(app);
        }

        private void LoadExtensionInDiscover()
        {
            var discover = serviceProvider.GetService<IAttributeDiscover>();
            discover.RegisterAssemblies(ExtensionManager.Assemblies);
        }
    }
}
