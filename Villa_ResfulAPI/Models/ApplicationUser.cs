using Microsoft.AspNetCore.Identity;

namespace Villa_ResfulAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
