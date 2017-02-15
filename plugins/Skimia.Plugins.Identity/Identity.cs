using System;
using System.Collections.Generic;
using System.Text;
using ExtCore.Data.EntityFramework.Sqlite;
using ExtCore.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Skimia.Plugins.Identity.Data.Models;
using Skimia.Plugins.Identity.Schema.Input;
using Skimia.Plugins.Identity.Schema.Result;

namespace Skimia.Plugins.Identity
{
    public class Identity : ExtensionBase
    {
        public override string Name => "Skimia.Extensions.Identity";

        public override IEnumerable<KeyValuePair<int, Action<IServiceCollection>>> ConfigureServicesActionsByPriorities
        {
            get
            {
                return new Dictionary<int, Action<IServiceCollection>>()
                {
                    [1000] = services =>
                    {
                        services.AddIdentity<User, Role>()
                            .AddEntityFrameworkStores<StorageContext>()
                            .AddDefaultTokenProviders();

                        services.Configure<IdentityOptions>(options =>
                        {
                            // Password settings
                            options.Password.RequireDigit = true;
                            options.Password.RequiredLength = 8;
                            options.Password.RequireNonAlphanumeric = false;
                            options.Password.RequireUppercase = false;
                            options.Password.RequireLowercase = false;

                            // Lockout settings
                            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                            options.Lockout.MaxFailedAccessAttempts = 10;

                            // Cookie settings
                            options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(150);
                            options.Cookies.ApplicationCookie.LoginPath = "/api/graph.ql";
                            options.Cookies.ApplicationCookie.LogoutPath = "/api/private.graph.ql";

                            // User settings
                            options.User.RequireUniqueEmail = true;
                        });

                    },
                    [2000] = services =>
                    {
                        //Graph types
                        services.AddSingleton<Schema.Identity>();
                        services.AddSingleton<UserRegistrationInputObject>();
                        services.AddSingleton<IdentityResultObject>();
                        services.AddSingleton<IdentityErrorObject>();

                    }
                };
            }
        }

        public override IEnumerable<KeyValuePair<int, Action<IApplicationBuilder>>> ConfigureActionsByPriorities
        {
            get
            {
                return new Dictionary<int, Action<IApplicationBuilder>>()
                {
                    [1000] = app =>
                    {
                        app.UseIdentity();
                    }
                };
            }
        }
    }
}
