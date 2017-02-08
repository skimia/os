using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skimia.Plugins.Identity.Schema.Input.Model
{
    public class UserRegistration
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
