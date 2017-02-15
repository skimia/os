using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skimia.Plugins.StaticFront.Configuration
{
    public class StaticFilesOptions
    {
        public static readonly string DefaultPath = "apps";

        /// <summary>
        /// Will be used only if AutomaticResolution is set to true 
        /// </summary>
        /// <value>
        /// The <see cref="Path"/> to store apps automatically resolved.
        /// </value>
        public string Path { get; set; } = DefaultPath;

        public bool AutomaticResolution { get; set; } = true;

        public IEnumerable<ApplicationOptions> Applications { get; set; }

    }
}
