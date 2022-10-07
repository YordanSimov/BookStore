using Microsoft.AspNetCore.Identity;

namespace ProjectDK.Models.Models.Users
{
    internal class UserRole : IdentityRole
    {
        public int UserId { get; set; }
    }
}
