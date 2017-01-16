using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQLCore.Type;

namespace Skimia.Extensions.GraphQl.Schema
{
    public class RootSchema : GraphQLSchema
    {
        public RootSchema()
        {
            var rootQuery = new Query();
            this.AddKnownType(rootQuery);
            this.Query(rootQuery);
        }
    }
}
