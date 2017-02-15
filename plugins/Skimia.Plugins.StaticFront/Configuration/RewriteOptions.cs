using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skimia.Plugins.StaticFront.Configuration
{
    public class RewriteOptions
    {
        public const string ApacheDefaultFile = "rewrite.apache";
        public string Apache { get; set; }

        public RewriteOptions()
        {
            Apache = ApacheDefaultFile;
        }
    }
}
