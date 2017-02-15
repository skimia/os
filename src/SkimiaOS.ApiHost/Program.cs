using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace SkimiaOS.ApiHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostOptions = GetHostOptions();

            IEnumerable<string> urls = hostOptions.Urls ?? new string[] { "http://::80", "http://0.0.0.0:80" };
            var host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .UseIISIntegration()
                .UseUrls(urls.ToArray())
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }

        protected static Configuration.HostOptions GetHostOptions()
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
