using System.Linq;
using GraphQLCore.Type;

namespace Skimia.Extensions.GraphQl.Schema
{
    public class Query : GraphQLObjectType
    {
        public Query() : base("Query", "Root query defintion")
        {
            this.Field("sum", (int[] numbers) => numbers.Sum());
        }
    }
}
