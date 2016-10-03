using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skimia.Extensions.Discovery.Attributes.Abstractions
{
    public interface IPropertyHandler
    {
        PropertyHandlerAttribute HandlerAttribute
        {
            get;
        }
    }
}
