using GraphQLCore.Type;
using Skimia.Extensions.GraphQl.Attributes;
using Skimia.Plugins.Identity.Data.Models;

namespace Skimia.Plugins.Identity.Schema.Result
{
    [GraphQlType]
    public class UserObject : GraphQLObjectType<User>
    {
        public UserObject() : base("User", "represents the authenticated user")
        {
            Field("username", e => e.UserName);

            Field("email", e => e.Email);
            Field("email_confirmed", e => e.EmailConfirmed);

            Field("phone_number", e => e.PhoneNumber);
            Field("phone_number_confirmed", e => e.PhoneNumberConfirmed);

            Field("normalized_username", e => e.NormalizedUserName);
            Field("normalized_email", e => e.NormalizedEmail);

            Field("two_factor_enabled", e => e.TwoFactorEnabled);

        }
    }
}
