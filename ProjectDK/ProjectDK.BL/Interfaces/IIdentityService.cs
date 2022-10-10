using Microsoft.AspNetCore.Identity;
using ProjectDK.Models.Models.Users;

namespace ProjectDK.BL.Interfaces
{
    public interface IIdentityService
    {
        Task<IdentityResult> CreateAsync(UserInfo user);

        Task<UserInfo?> CheckUserAndPassword(string userName,string password);

        Task<IEnumerable<string>> GetUserRoles(UserInfo user);
    }
}
