using GraphQLCore.Type;
using Microsoft.AspNetCore.Identity;
using Skimia.Extensions.GraphQl.Attributes;

namespace Skimia.Plugins.Identity.Schema.Result
{
    [GraphQlType]
    public class IdentityErrorObject : GraphQLObjectType<IdentityError>
    {
        public IdentityErrorObject() : base("IdentityError", "error in an identity operation")
        {
            Field("code", e => e.Code);
            Field("description", e => e.Description);
        }
    }
}
