using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExtCore.Data.Models.Abstractions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Skimia.Plugins.Identity.Data.Models
{
    public class User : IdentityUser, IEntity
    {
    }
}
