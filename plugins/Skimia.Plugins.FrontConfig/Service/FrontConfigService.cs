using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Skimia.Plugins.FrontConfig.Configuration;

namespace Skimia.Plugins.FrontConfig.Service
{
    public class FrontConfigService
    {
        private readonly ILogger _logger;
        private readonly IOptions<FrontConfigOptions> _frontConfigOptions;

        private readonly IEnumerable<Configuration.Front.Configuration> _pluginsConfigurations;

        public FrontConfigService(
            IOptions<FrontConfigOptions> frontConfigOptions,
            ILoggerFactory loggerFactory
        )
        {
            _frontConfigOptions = frontConfigOptions;
            _logger = loggerFactory.CreateLogger<FrontConfigService>();
            _pluginsConfigurations = GetList();
        }

        public IEnumerable<Configuration.Front.Configuration> List()
        {
            return _pluginsConfigurations;
        }

        private IEnumerable<Configuration.Front.Configuration> GetList()
        {
            var configurations = new List<Configuration.Front.Configuration>();

            var baseDir =
                new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), _frontConfigOptions.Value.Path));

            if (baseDir.Exists)
            {
                IEnumerable<DirectoryInfo> appsDirectories = baseDir.EnumerateDirectories();

                foreach (var appDirectory in appsDirectories)
                {
                    try
                    {
                        if (appDirectory.GetFiles("config.json").Length > 0)
                        {
                            var builder = new ConfigurationBuilder()
                                .SetBasePath(appDirectory.FullName)
                                .AddJsonFile("config.json");

                            var pluginConfig = builder.Build();

                            configurations.Add(pluginConfig.Get<Configuration.Front.Configuration>());
                        }

                    }
                    catch (Exception ex)
                    { 
                        _logger.LogError(400, ex, "Loding Plugins Configurations Errors " + appDirectory.Name);
                    }
                }
            }

            return configurations;
        }
    }
}
