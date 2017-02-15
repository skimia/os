using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skimia.Plugins.StaticFront.Configuration
{
    public class StaticFilesOptions
    {
        public string Path { get; set; }
        public bool AutomaticResolution { get; set; }
        public IEnumerable<ApplicationOptions> Applications { get; set; }

        public StaticFilesOptions()
        {
            Path = "apps";
            AutomaticResolution = true;
        }
    }
}
