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
