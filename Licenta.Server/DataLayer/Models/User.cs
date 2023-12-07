using Microsoft.AspNetCore.Identity;

namespace Licenta.Server.DataLayer.Models
{
    public class User : IdentityUser
    {
        public Role Role { get; set; }
    }
}
