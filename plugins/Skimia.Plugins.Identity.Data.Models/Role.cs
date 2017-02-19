using ExtCore.Data.Models.Abstractions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Skimia.Plugins.Identity.Data.Models
{
    public class Role : IdentityRole, IEntity
    {
    }
}
