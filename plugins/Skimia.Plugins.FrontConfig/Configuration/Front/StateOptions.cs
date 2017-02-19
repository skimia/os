using System.Collections.Generic;

namespace Skimia.Plugins.FrontConfig.Configuration.Front
{
    public class StateOptions
    {
        public string Name { get; set; } = null;

        public string Url { get; set; } = null;

        public IEnumerable<ViewOption> Views { get; set; } = null;

        public IEnumerable<string> Assets { get; set; } = null;
    }
}