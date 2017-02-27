using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace SkimiaOS.ApiHost
{
    public class Program
    {
        private static readonly Dictionary<string,string> DistFiles = new Dictionary<string, string>()
        {
            {"config/host.json",      "config/dist/host.json" },
            {"config/framework.json", "config/dist/framework.json" },
            {"config/plugins.json",   "config/dist/plugins.json" },
        };
        public static void Main(string[] args)
        {
            BuildDist();

            var hostOptions = GetHostOptions();

            string urls = hostOptions.Urls ?? "http://localhost:5000";
            var host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .UseIISIntegration()
                .UseUrls(urls)
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }

        private static void BuildDist()
        {
            foreach (var distFile in DistFiles)
            {
                if (!File.Exists(distFile.Key) && File.Exists(distFile.Value))
                {
                    File.Copy(distFile.Value,distFile.Key);
                }
            }
        }

        private static Configuration.HostOptions GetHostOptions()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config/host.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables();
                
            var configuration = builder.Build();

            return configuration.GetSection("Host").Get<Configuration.HostOptions>() ?? new Configuration.HostOptions();
        }
    }
}
