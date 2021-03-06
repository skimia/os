﻿using System;
using ExtCore.Data.EntityFramework.Sqlite;
using ExtCore.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Skimia.Extensions.Discovery.Attributes;
using Skimia.Extensions.Discovery.Attributes.Abstractions;
using Skimia.Extensions.Messages;

namespace SkimiaOS.ApiHost {
    public class Startup : ExtCore.WebApplication.Startup {
        public Startup (IHostingEnvironment env, IServiceProvider serviceProvider): base (serviceProvider) {
                
            var builder = new ConfigurationBuilder ()
                .SetBasePath (env.ContentRootPath)
                .AddJsonFile ("config/framework.json", optional : true, reloadOnChange : true)
                .AddJsonFile ("config/serilog.json", optional : true, reloadOnChange : true)
                .AddJsonFile ("config/plugins.json", optional : true, reloadOnChange : true)
                .AddJsonFile ($"config/framework.{env.EnvironmentName}.json", optional : true)
                .AddEnvironmentVariables ();

            configurationRoot = builder.Build ();

            Log.Logger = CreateLogger();

            this.serviceProvider.GetService<ILoggerFactory> ()
                .AddConsole (configurationRoot.GetSection ("Logging"))
                .AddSerilog();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public override void ConfigureServices (IServiceCollection services) {
            // Add framework services.
            services.AddMvc ();
            services.AddCors (options => {
                options.AddPolicy ("AllowAnyOriginHeadersAndMethods",
                    builder => {
                        builder.AllowAnyOrigin ()
                            .AllowAnyHeader ()
                            .AllowAnyMethod ()
                            .AllowCredentials ();
                    }
                );
            });
            services.AddDiscovery ();
            services.AddMessages ();

            base.ConfigureServices (services);

            //ensure maindbcontext from ExtCore
            var s = ExtensionManager.GetInstance<Storage> ();
            services.AddSingleton (s.StorageContext);

            RebuildServiceProvider (services);
            //sure db is OK!
            s.StorageContext.Database.EnsureCreated ();

            LoadExtensionInDiscover ();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public override void Configure (IApplicationBuilder app) {
            base.Configure (app);
            //can be mloved in another pluggin
            app.UseMvc ();
        }

        private Serilog.ILogger CreateLogger()
        {
            return new LoggerConfiguration ()
                .ReadFrom.ConfigurationSection(configurationRoot.GetSection ("Serilog"))
                .CreateLogger ();
        }
        private void RebuildServiceProvider (IServiceCollection services) {
            serviceProvider = services.BuildServiceProvider ();
            foreach (IExtension extension in ExtensionManager.Extensions)
            extension.SetServiceProvider (serviceProvider);
        }

        private void LoadExtensionInDiscover () {
            var discover = serviceProvider.GetRequiredService<IAttributeDiscover> ();
            discover.RegisterAssemblies (ExtensionManager.Assemblies);
        }
    }
}