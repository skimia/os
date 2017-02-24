using System.Collections.Generic;

namespace Skimia.Plugins.FrontConfig.Configuration.Front
{
    public class Configuration
    {
        public string Name { get; set; } = null;

        public string Version { get; set; } = null;

        public string Otherwise { get; set; } = null;

        public IEnumerable<string> AssetsBeforeLoad { get; set; } = null;

        public IEnumerable<StateOptions> Routing { get; set; } = null;
    }
}
