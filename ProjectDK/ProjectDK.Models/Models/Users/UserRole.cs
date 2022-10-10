using Microsoft.AspNetCore.Identity;

namespace ProjectDK.Models.Models.Users
{
    public class UserRole : IdentityRole
    {
        public int UserId { get; set; }
    }
}
