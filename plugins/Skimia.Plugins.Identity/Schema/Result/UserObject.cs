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
            this.Field("username", e => e.UserName);

            this.Field("email", e => e.Email);
            this.Field("email_confirmed", e => e.EmailConfirmed);

            this.Field("phone_number", e => e.PhoneNumber);
            this.Field("phone_number_confirmed", e => e.PhoneNumberConfirmed);

            this.Field("normalized_username", e => e.NormalizedUserName);
            this.Field("normalized_email", e => e.NormalizedEmail);

            this.Field("two_factor_enabled", e => e.TwoFactorEnabled);

        }
    }
}
