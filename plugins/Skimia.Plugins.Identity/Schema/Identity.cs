using System.Threading.Tasks;
using GraphQLCore.Type;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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

        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;


        public Identity(
            IHttpContextAccessor httpContextAccessor,
            Mutation mutation,
            Query query,
            UserManager<User> userManager,
            SignInManager<User> signInManager, 
            ILoggerFactory loggerFactory) : base("Identity", "Identity Plugin GraphQl Namespace")
        {
            _userManager = userManager;
            _signInManager = signInManager;

            _logger = loggerFactory.CreateLogger<Identity>();
            _httpContextAccessor = httpContextAccessor;

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
            var dbUser = new User { UserName = user.Username, Email = user.Email };
            var result = await _userManager.CreateAsync(dbUser, user.Password);

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
            var result = await _signInManager.PasswordSignInAsync(username,password,false,true);
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
            return await _userManager.GetUserAsync(this._httpContextAccessor.HttpContext.User);
        }
    }
}
