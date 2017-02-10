using System;
using System.Linq;
using ExtCore.Data.Abstractions;
using GraphQLCore.Type;
using Microsoft.AspNetCore.Identity;
using Skimia.Extensions.GraphQl.Attributes;
using Skimia.Extensions.GraphQl.Schema;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Skimia.Plugins.Identity.Data.Models;
using Skimia.Plugins.Identity.Schema.Input.Model;

namespace Skimia.Plugins.Identity.Schema
{
    //Inject Type in RootSchema
    [GraphQlType]
    public class Identity : GraphQLObjectType
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;
        public Identity(
            Mutation mutation,
            UserManager<User> userManager,
            SignInManager<User> signInManager, 
            ILoggerFactory loggerFactory) : base("Identity", "Identity Plugin GraphQl Namespace")
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<Identity>();

            this.Field("register", (UserRegistration user) => Register(user));
            this.Field("authenticate", (string username, string password) => Authenticate(username, password));

            mutation.Field("identity", () => this);
        }

        private IdentityResult Register(UserRegistration user)
        {
            var dbUser = new User { UserName = user.Username, Email = user.Email };
            var result = _userManager.CreateAsync(dbUser, user.Password);
            result.Wait();

            if (result.Result.Succeeded)
            {
                _logger.LogInformation(1, "User register.");
                return result.Result;
            }
            _logger.LogWarning(2, "Invalid register attempt.");
            return result.Result;
        }

        private string Authenticate(string username, string password)
        {
            var result = _signInManager.PasswordSignInAsync(username,password,false,true);
            result.Wait();
            if (result.Result.Succeeded)
            {
                _logger.LogInformation(1, "User logged in.");
                return CreateToken();
            }
            _logger.LogWarning(2, "Invalid login attempt.");
            return "error";
        }

        private static string CreateToken()
        {
            var time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            var key = Guid.NewGuid().ToByteArray();
            return Convert.ToBase64String(time.Concat(key).ToArray());
        }
    }
}
