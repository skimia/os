using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace SkimiaOS.ApiHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .UseIISIntegration()
                .UseUrls("http://::80", "http://0.0.0.0:80")
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
