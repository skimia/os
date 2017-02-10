using System;
using System.Collections.Generic;
using System.IO;
using ExtCore.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Rewrite.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace Skimia.Plugins.StaticFront
{
    public class StaticFront : ExtensionBase
    {
        public override string Name => "Skimia.Extensions.StaticFront";

        public override IEnumerable<KeyValuePair<int, Action<IApplicationBuilder>>> ConfigureActionsByPriorities
        {
            get
            {
                return new Dictionary<int, Action<IApplicationBuilder>>()
                {
                    [2000] = appBuilder =>
                    {
                        //config to be deplaced in apps with automatic resolution (in this case 
                        // get config from local config file (in dir) (keeps no exposed)
                        // for configuration use simple yml files (deny all *.yml|*.yaml)
                        var baseDir = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), @"apps"));
                        IEnumerable<DirectoryInfo> apps = baseDir.EnumerateDirectories();

                        foreach (var app in apps)
                        {
                            
                            PathString baseUrl = app.Name == "os.web" ? new PathString("/"): new PathString("/"+ app.Name) ;
                            var fileProvider = new PhysicalFileProvider(app.FullName);

                            AddApplication(appBuilder, fileProvider, baseUrl);
                        }

                        
                        
                       
                    }
                };
            }
        }

        public void AddApplication(IApplicationBuilder app, IFileProvider fileProvider, PathString baseUrl)
        {
            baseUrl = baseUrl.Equals(new PathString("/")) ? null : baseUrl;
            //if has apache config
            if (fileProvider.GetFileInfo("rewrite.apache").Exists)
            {
                var options = new RewriteOptions()
                    .AddApacheModRewrite(fileProvider, "rewrite.apache");
                options.StaticFileProvider = fileProvider;
                app.UseRewriter(options);
            }
            else
            {
                app.UseDefaultFiles();
            }
            

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = fileProvider,
                RequestPath = baseUrl
            });

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
