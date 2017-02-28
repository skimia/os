using System;
using System.Threading.Tasks;
using GraphQLCore.Type;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Skimia.Extensions.GraphQl.Attributes;
using Skimia.Extensions.GraphQl.Schema;
using Microsoft.Extensions.Logging;
using Skimia.Extensions.GraphQl.Type;
using Skimia.Plugins.Identity.Data.Models;
using Skimia.Plugins.Identity.Schema.Input.Model;

namespace Skimia.Plugins.Identity.Schema
{
    //Inject Type in RootSchema
    [GraphQlType]
    public class Identity : GraphQLObjectType
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;

        public Identity(
            Mutation mutation,
            Query query,
            IServiceProvider serviceProvider,
            ILoggerFactory loggerFactory) : base("Identity", "Identity Plugin GraphQl Namespace")
        {
            _serviceProvider = serviceProvider;
            _logger = loggerFactory.CreateLogger<Identity>();

            this.Field("register", (UserRegistration user) => Register(user).GetAwaiter().GetResult());

            this.Field("authenticate", 
                (string username, string password) => 
                    Authenticate(username, password).GetAwaiter().GetResult()
                );


            query.SecureField("me",
                () => GetUserAsync().GetAwaiter().GetResult()
            );
                

            mutation.Field("identity", () => this);
        }

        private async Task<IdentityResult> Register(UserRegistration user)
        {
            var dbUser = new User {UserName = user.Username, Email = user.Email};

            //Take care if your schemas is singleton and yours services if scoped it can be disposed on the execution replay,
            //get the last fresh instance with service provider
            var userManager = _serviceProvider.GetRequiredService<UserManager<User>>();

            var result = await userManager.CreateAsync(dbUser, user.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation(1, "User register.");

                return result;
            }

            _logger.LogWarning(2, "Invalid register attempt.");

            return result;

        }

        /// <summary>
        /// Need to return an AuthenticateResult to can return SignIn Error Status & User if succeded
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task<string> Authenticate(string username, string password)
        {
            var signInManager = _serviceProvider.GetRequiredService<SignInManager<User>>();

            var result = await signInManager.PasswordSignInAsync(username,password,false,true);

            if (result.Succeeded)
            {
                _logger.LogInformation(1, "User logged in.");

                return (await GetUserAsync()).UserName;
                //return GetUserAsync().GetAwaiter().GetResult();
            }

            _logger.LogWarning(2, "Invalid login attempt.");
            return "error";
        }

        private async Task<User> GetUserAsync()
        {
            var userManager = _serviceProvider.GetRequiredService<UserManager<User>>();

            return await userManager.GetUserAsync(_serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext.User);
        }
    }
}
