using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExtCore.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Skimia.Plugins.StaticFront.Configuration;
using RewriteOptions = Microsoft.AspNetCore.Rewrite.RewriteOptions;

namespace Skimia.Plugins.StaticFront
{
    public class StaticFront : ExtensionBase
    {
        public override string Name => "Skimia.Extensions.StaticFront";

        public override IEnumerable<KeyValuePair<int, Action<IApplicationBuilder>>> ConfigureActionsByPriorities
        {
            get
            {
                //Adds An extension to handle configuration validation
                // see http://stackoverflow.com/a/32794820/7043816
                var staticFilesConfig = configurationRoot.GetSection("Plugins:StaticFiles").Get<StaticFilesOptions>();

                if (staticFilesConfig == null)
                {
                    return new Dictionary<int, Action<IApplicationBuilder>>();
                }

                var apps = new Dictionary<ApplicationOptions,IFileProvider>();

                if (staticFilesConfig.AutomaticResolution)
                {

                    var baseDir = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), staticFilesConfig.Path));

                    if (baseDir.Exists)
                    {
                        IEnumerable<DirectoryInfo> appsDirectories = baseDir.EnumerateDirectories();

                        foreach (var appDirectory in appsDirectories)
                        {
                            var appOptions = new ApplicationOptions()
                            {
                                BindTo = "/" + appDirectory.Name,
                                Name = appDirectory.Name,
                                Path = appDirectory.FullName

                            };

                            var fileProvider = new PhysicalFileProvider(appDirectory.FullName);

                            apps.Add(appOptions, fileProvider);
                        }
                    }
                    
                }
                else if(staticFilesConfig.Applications != null && staticFilesConfig.Applications.Any())
                {
                    foreach (var webApp in staticFilesConfig.Applications)
                    {
                        webApp.Path = Path.Combine(Directory.GetCurrentDirectory(), webApp.Path);
                        if (webApp.Path != null && ( new DirectoryInfo(webApp.Path).Exists ))
                        {
                            var fileProvider = new PhysicalFileProvider(webApp.Path);
                            apps.Add(webApp, fileProvider);
                        }
                        
                    }
                    
                }

                return new Dictionary<int, Action<IApplicationBuilder>>()
                {

                    [3000] = app =>
                    {
                        foreach (var appDefinition in apps)
                        {
                            AddApplicationRewrite(app, appDefinition.Key, appDefinition.Value);
                        }
                    },
                    [4000] = app =>
                    {
                        foreach (var appDefinition in apps)
                        {
                            AddApplicationStatics(app, appDefinition.Key, appDefinition.Value);
                        }
                    }
                };
            }
        }

        private void AddApplicationStatics(IApplicationBuilder app, ApplicationOptions webApp, IFileProvider fileProvider)
        {
            var baseUrl = webApp.BindTo.Equals(new PathString("/")) ? null : webApp.BindTo;
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = fileProvider,
                RequestPath = baseUrl
            });
        }

        private void AddApplicationRewrite(IApplicationBuilder app, ApplicationOptions webApp, IFileProvider fileProvider)
        {
            var baseUrl = webApp.BindTo.Equals(new PathString("/")) ? null : webApp.BindTo;
            //if has apache config
            if ( !string.IsNullOrEmpty(webApp.Rewrite?.Apache) && fileProvider.GetFileInfo(webApp.Rewrite.Apache).Exists)
            {
                var options = new RewriteOptions()
                    .AddApacheModRewrite(fileProvider, webApp.Rewrite.Apache);
                options.StaticFileProvider = fileProvider;
                app.UseRewriter(options);
            }
            else
            {
                app.UseDefaultFiles(new DefaultFilesOptions()
                {
                    FileProvider = fileProvider,
                    RequestPath = baseUrl
                });
            }

            //if directory
            if (this.serviceProvider.GetService<IHostingEnvironment>().IsDevelopment())
            {
                app.UseDirectoryBrowser(new DirectoryBrowserOptions()
                {
                    FileProvider = fileProvider,
                    RequestPath = baseUrl
                });
            }
        }
    }
}
