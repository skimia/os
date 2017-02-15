using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skimia.Plugins.StaticFront.Configuration
{
    public class ApplicationOptions
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public string BindTo { get; set; }

        public RewriteOptions Rewrite { get; set; } = new RewriteOptions();
    }
}
