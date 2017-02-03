using System;
using System.Linq;
using System.Security.Cryptography;
using Skimia.Extensions.GraphQl.Attributes;
using GraphQLCore.Type;
using Skimia.Extensions.GraphQl.Schema;

namespace Skimia.Plugins.Auth.Schema
{
    //Inject Type in RootSchema
    [GraphQlType]
    public class Auth : GraphQLObjectType
    {
        public Auth(Query query) : base("Auth", "Auth Plugin GraphQl Namespace")
        {
            this.Field("authenticate", (string login, string password) => CreateToken());

            query.Field("auth", () => this);
        }

        private static string CreateToken()
        {
            var time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            var key = Guid.NewGuid().ToByteArray();
            return Convert.ToBase64String(time.Concat(key).ToArray());
        }
    }
}
