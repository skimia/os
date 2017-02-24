using GraphQLCore.Type;
using Microsoft.AspNetCore.Identity;
using Skimia.Extensions.GraphQl.Attributes;

namespace Skimia.Plugins.Identity.Schema.Result
{
    [GraphQlType]
    public class IdentityResultObject :GraphQLObjectType<IdentityResult>
    {
        public IdentityResultObject() : base("IdentityResult", "result of an identity operation")
        {
            Field("succeeded", e => e.Succeeded);
            Field("errors", e => e.Errors);
        }
    }
}
