using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProjectDK.DL.Interfaces;
using ProjectDK.Models.Models.Users;
using System.Data.SqlClient;

namespace ProjectDK.DL.Repositories.MsSQL
{
    public class UserInfoStore : IUserPasswordStore<UserInfo>
    {
        private readonly ILogger<UserInfoStore> _logger;
        private readonly IConfiguration _configuration;
        public UserInfoStore(IConfiguration configuration, ILogger<UserInfoStore> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IdentityResult> CreateAsync(UserInfo user, CancellationToken cancellationToken)
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    var result = await connection.QueryFirstOrDefaultAsync<UserInfo>
                        ("SELECT * FROM UserInfo WHERE UserID = @UserId", new { UserId = user.UserId });
                    if (result == null)
                    {
                        await connection.ExecuteAsync
        ("INSERT INTO UserInfo(UserId,DisplayName,UserName,Email,Password,CreatedDate)" +
        " VALUES (@UserId,@DisplayName,@UserName,@Email,@Password,@CreatedDate", user);
                        return IdentityResult.Success;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return IdentityResult.Failed(new IdentityError() { Description = ex.Message});
            }
            return IdentityResult.Failed(new IdentityError() { Description = "User already exists" });
        }

        public Task<IdentityResult> DeleteAsync(UserInfo user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<UserInfo> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<UserInfo> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    var result = await connection.QueryFirstOrDefaultAsync<UserInfo>
                         ("SELECT * FROM UserInfo WHERE UserName = @Username", new { Username = normalizedUserName });
                    return await Task.FromResult(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        public Task<string> GetNormalizedUserNameAsync(UserInfo user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(UserInfo user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetUserIdAsync(UserInfo user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.UserId.ToString());
        }


        public async Task<string> GetUserNameAsync(UserInfo user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.UserName);
        }

        public Task<bool> HasPasswordAsync(UserInfo user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(UserInfo user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetPasswordHashAsync(UserInfo user, string passwordHash, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetUserNameAsync(UserInfo user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(UserInfo user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
