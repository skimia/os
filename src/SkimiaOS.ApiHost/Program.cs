using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace SkimiaOS.ApiHost
{
    public class Program
    {
        private static readonly Dictionary<string,string> DistFiles = new Dictionary<string, string>()
        {
            {"config.json", "config.dist.json" },
        };
        public static void Main(string[] args)
        {
            BuildDist();

            var hostOptions = GetHostOptions();

            IEnumerable<string> urls = hostOptions.Urls ?? new string[] { "http://localhost:5000" };
            var host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .UseIISIntegration()
                .UseUrls(urls.ToArray())
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
                .AddJsonFile("config.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables();
                
            var configuration = builder.Build();

            return configuration.GetSection("Host").Get<Configuration.HostOptions>() ?? new Configuration.HostOptions();
        }
    }
}
