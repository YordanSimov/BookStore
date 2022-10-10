using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProjectDK.Models.Models.Users;
using System.Data;
using System.Data.SqlClient;

namespace ProjectDK.DL.Repositories.MsSQL
{
    public class UserInfoStore : IUserPasswordStore<UserInfo>, IUserRoleStore<UserInfo>
    {
        private readonly ILogger<UserInfoStore> _logger;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<UserInfo> _passwordHasher;
        public UserInfoStore(IConfiguration configuration,
            ILogger<UserInfoStore> logger,
            IPasswordHasher<UserInfo> passwordHasher)
        {
            _configuration = configuration;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        public Task AddToRoleAsync(UserInfo user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> CreateAsync(UserInfo user, CancellationToken cancellationToken)
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    var result = await connection.QueryFirstOrDefaultAsync<UserInfo>
                        ("SELECT * FROM UserInfo WHERE UserId = @UserId", new { UserId = user.UserId });
                    if (result == null)
                    {
                        user.Password = _passwordHasher.HashPassword(user, user.Password);
                        await connection.ExecuteAsync
        ("INSERT INTO UserInfo(DisplayName,UserName,Email,Password,CreatedDate)" +
        " VALUES (@DisplayName,@UserName,@Email,@Password,@CreatedDate)", user);
                        return IdentityResult.Success;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return IdentityResult.Failed(new IdentityError() { Description = ex.Message });
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

        public async Task<string> GetPasswordHashAsync(UserInfo user, CancellationToken cancellationToken)
        {
            await using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            await conn.OpenAsync(cancellationToken);
            return await conn.QueryFirstOrDefaultAsync<string>
                ("SELECT Password FROM UserInfo WITH (NOLOCK) WHERE UserId = @userId", new { userId = user.UserId });
        }

        public async Task<IList<string>> GetRolesAsync(UserInfo user, CancellationToken cancellationToken)
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    var result = await connection.QueryAsync<string>
           ("SELECT r.RoleName FROM Roles r WHERE r.Id IN(SELECT ur.Id FROM UserRoles ur WHERE ur.UserId = @UserId)", new { UserId = user.UserId});
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        public async Task<string> GetUserIdAsync(UserInfo user, CancellationToken cancellationToken)
        {
            await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                var result = await connection.QueryFirstOrDefaultAsync<UserInfo>
                    ("SELECT * FROM UserInfo WITH (NOLOCK) WHERE UserId = @UserId", new { UserId = user.UserId });

                return result?.UserId.ToString();
            }
        }


        public async Task<string> GetUserNameAsync(UserInfo user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.UserName);
        }

        public Task<IList<UserInfo>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> HasPasswordAsync(UserInfo user, CancellationToken cancellationToken)
        {
            return !string.IsNullOrEmpty(await GetPasswordHashAsync(user, cancellationToken));
        }

        public Task<bool> IsInRoleAsync(UserInfo user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(UserInfo user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task SetNormalizedUserNameAsync(UserInfo user, string normalizedName, CancellationToken cancellationToken)
        {

        }

        public async Task SetPasswordHashAsync(UserInfo user, string passwordHash, CancellationToken cancellationToken)
        {
            await using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            await conn.OpenAsync(cancellationToken);
            await conn.ExecuteAsync
                (@"UPDATE UserInfo SET Password = @passwordHash WHERE UserId = @userId", new { passwordHash = passwordHash, userId = user.UserId });
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
