using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProjectDK.Models.Models.Users;
using System.Data.SqlClient;

namespace ProjectDK.DL.Repositories.MsSQL
{
    public class UserRoleStore : IRoleStore<UserRole>
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserRoleStore> _logger;

        public UserRoleStore(IConfiguration configuration, ILogger<UserRoleStore> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IdentityResult> CreateAsync(UserRole role, CancellationToken cancellationToken)
        {
            await using(var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    await connection.OpenAsync(cancellationToken);

                    var result = await connection.ExecuteAsync
                        (@"INSERT INTO UserRoles ([Id], [RoleId],[UserId]) VALUES (@Id,@RoleId,@UserId)", role);
                    return IdentityResult.Success;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error in {nameof(CreateAsync)}");
                    return IdentityResult.Failed();

                }
            }
        }

        public Task<IdentityResult> DeleteAsync(UserRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<UserRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<UserRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    return await connection.QueryFirstOrDefaultAsync<UserRole>
           ("SELECT ur.Id,ur.RoleId,ur.UserId,r.RoleName as Name FROM UserRole ur WITH (NOLOCK) JOIN Roles r ON ur.RoleId = r.Id " +
           "WHERE r.RoleName = @normalizedRoleName",
                         new { normalizedRoleName = normalizedRoleName });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        public Task<string> GetNormalizedRoleNameAsync(UserRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleIdAsync(UserRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleNameAsync(UserRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedRoleNameAsync(UserRole role, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetRoleNameAsync(UserRole role, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(UserRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
