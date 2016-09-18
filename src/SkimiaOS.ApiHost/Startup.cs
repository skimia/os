using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

            base.ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public override void Configure(IApplicationBuilder app)
        {
            app.UseMvc();

            base.Configure(app);
        }
    }
}
