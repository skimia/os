using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQLCore.Type;

namespace Skimia.Extensions.GraphQl.Schema
{
    public class Mutation : GraphQLObjectType
    {
        public Mutation() : base("Mutation", "Root mutation defintion")
        {
        }
    }
}
