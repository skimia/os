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
using Microsoft.Extensions.Logging;
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

                var apps = new Dictionary<ApplicationOptions, IFileProvider>();

                if (staticFilesConfig.AutomaticResolution)
                {
                    this.logger.LogInformation("Automatic Front Applications Resolution");
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


                            this.logger.LogInformation("Add Front Application {0}", appDirectory.Name);

                            apps.Add(appOptions, fileProvider);
                        }
                    }
                    else
                    {
                        this.logger.LogWarning("Directory {0} does not exists", baseDir.Name);
                    }

                }
                else if (staticFilesConfig.Applications != null && staticFilesConfig.Applications.Any())
                {
                    foreach (var webApp in staticFilesConfig.Applications)
                    {
                        webApp.Path = Path.Combine(Directory.GetCurrentDirectory(), webApp.Path);
                        if (webApp.Path != null && (new DirectoryInfo(webApp.Path).Exists))
                        {
                            var fileProvider = new PhysicalFileProvider(webApp.Path);
                            apps.Add(webApp, fileProvider);
                        }
                        else
                        {
                            this.logger.LogWarning("Invalid path {0} for app {1}  does not exists", webApp.Path, webApp.Name);
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

            this.logger.LogInformation("Bind Front Application {0} to : {1}", webApp.Name, webApp.BindTo);
        }

        private void AddApplicationRewrite(IApplicationBuilder app, ApplicationOptions webApp, IFileProvider fileProvider)
        {
            var baseUrl = webApp.BindTo.Equals(new PathString("/")) ? null : webApp.BindTo;
            //if has apache config
            if (!string.IsNullOrEmpty(webApp.Rewrite?.Apache) && fileProvider.GetFileInfo(webApp.Rewrite.Apache).Exists)
            {
                var options = new RewriteOptions()
                    .AddApacheModRewrite(fileProvider, webApp.Rewrite.Apache);
                options.StaticFileProvider = fileProvider;
                app.UseRewriter(options);

                this.logger.LogInformation("Use apache rewrite file ({0}) for Front Application {1}", webApp.Rewrite.Apache, webApp.Name);
            }
            else
            {
                app.UseDefaultFiles(new DefaultFilesOptions()
                {
                    FileProvider = fileProvider,
                    RequestPath = baseUrl
                });

                this.logger.LogInformation("Use default index files for Front Application {1}", webApp.Name);
            }

            //if directory
            if (serviceProvider.GetService<IHostingEnvironment>().IsDevelopment())
            {
                app.UseDirectoryBrowser(new DirectoryBrowserOptions()
                {
                    FileProvider = fileProvider,
                    RequestPath = baseUrl
                });
                
                this.logger.LogInformation("Add directories index for Front Application {1}", webApp.Name);
            }
        }
    }
}
