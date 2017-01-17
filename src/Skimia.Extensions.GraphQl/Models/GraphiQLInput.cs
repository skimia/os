using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skimia.Extensions.GraphQl.Models
{
    public class GraphiQLInput
    {
        public string query { get; set; }
        public string variables { get; set; }
    }
}
