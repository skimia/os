using GraphQLCore.Type;
using Skimia.Extensions.GraphQl.Attributes;
using Skimia.Plugins.Identity.Schema.Input.Model;

namespace Skimia.Plugins.Identity.Schema.Input
{
    [GraphQlType]
    public class UserRegistrationInputObject : GraphQLInputObjectType<UserRegistration>
    {
        public UserRegistrationInputObject() : base("InputUserRegister", "Input object for create an user")
        {
            this.Field("username", e => e.Username);
            this.Field("email", e => e.Email);
            this.Field("password", e => e.Password);
        }
    }
}
