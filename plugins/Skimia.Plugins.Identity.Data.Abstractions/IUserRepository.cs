using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Identity;
using Skimia.Plugins.Identity.Data.Models;

namespace Skimia.Plugins.Identity.Data.Abstractions
{
    public interface IUserRepository : IUserStore<User>, IRepository
    {
    }
}
