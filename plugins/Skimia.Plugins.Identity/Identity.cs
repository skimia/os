using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
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
                    [1000] = collection =>
                    {
                        var s = ExtensionManager.GetInstance<Storage>();
                        collection.AddSingleton<StorageContext>(s.StorageContext);

                        collection.AddIdentity<User, IdentityRole>()
                            .AddEntityFrameworkStores<StorageContext>()
                            .AddDefaultTokenProviders();

                        collection.AddSingleton<Schema.Identity>();
                        collection.AddSingleton<UserRegistrationInputObject>();
                        collection.AddSingleton<IdentityResultObject>();
                        collection.AddSingleton<IdentityErrorObject>();

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
                    [2000] = appBuilder =>
                    {
                        appBuilder.UseIdentity();
                       
                    }
                };
            }
        }
    }
}
