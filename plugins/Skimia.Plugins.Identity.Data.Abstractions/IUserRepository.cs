using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Skimia.Plugins.Identity.Data.Models;

namespace Skimia.Plugins.Identity.Data.Abstractions
{
    public interface IUserRepository : IUserStore<User>, IRepository
    {
    }
}
