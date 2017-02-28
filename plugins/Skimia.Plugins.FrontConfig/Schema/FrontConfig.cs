using System.Collections.Generic;
using System.Linq;
using GraphQLCore.Type;
using Microsoft.Extensions.Logging;
using Skimia.Extensions.GraphQl.Attributes;
using Skimia.Extensions.GraphQl.Schema;
using Skimia.Plugins.FrontConfig.Schema.Result;
using Skimia.Plugins.FrontConfig.Service;

namespace Skimia.Plugins.FrontConfig.Schema
{
    [GraphQlType]
    public class FrontConfig : GraphQLObjectType
    {
        private readonly ILogger _logger;
        private readonly FrontConfigService _frontConfigService;

        public FrontConfig(
            Query query,
            FrontConfigService frontConfigService,
            ILoggerFactory loggerFactory) : base("FrontConfig", "Front json Configuration Manager")
        {
            _logger = loggerFactory.CreateLogger<FrontConfig>();
            _frontConfigService = frontConfigService;

            this.Field("plugins", (string has)=> GetPlugins(has));

            this.Field("otherwise", ()=> Otherwise());

            query.Field("frontConfig",
                () => this
            );
        }

        private IEnumerable<Configuration.Front.Configuration> GetPlugins(string has)
        {
            var configurations = _frontConfigService.List();
            if (has != null && FrontConfiguration.HasField(has))
            {
                return configurations.Where(e => FrontConfiguration.HasFieldValue(has, e));
            }
            return configurations;
        }

        /// <summary>
        /// get the otherwise for angular ui router
        /// </summary>
        /// <returns>Only one otherwise</returns>
        /// <remarks>needs explaination if two module declare otherwise</remarks>
        private string Otherwise()
        {
            return _frontConfigService.List().First(c => c.Otherwise != null).Otherwise;
        }
    }
}
