using ProjectDK.Models.Models.Users;

namespace ProjectDK.DL.Interfaces
{
    public interface IUserInfoRepository
    {
        Task<UserInfo?> GetUserInfoAsync(string email, string password);
    }
}
