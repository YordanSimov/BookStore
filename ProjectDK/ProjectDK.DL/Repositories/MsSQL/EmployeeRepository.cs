using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProjectDK.DL.Interfaces;
using ProjectDK.Models.Models.Users;
using System.Data.SqlClient;

namespace ProjectDK.DL.Repositories.MsSQL
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ILogger<EmployeeRepository> _logger;
        private readonly IConfiguration _configuration;
        public EmployeeRepository(IConfiguration configuration, ILogger<EmployeeRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task AddEmployee(Employee employee)
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    await connection.ExecuteAsync
                        (@"INSERT INTO Employee 
                        ([EmployeeID]
                       ,[NationalIDNumber]
                       ,[EmployeeName]
                       ,[LoginID]
                       ,[JobTitle]
                       ,[BirthDate]
                       ,[MaritalStatus]
                       ,[Gender]
                       ,[HireDate]
                       ,[VacationHours]
                       ,[SickLeaveHours]
                       ,[rowguid]
                       ,[ModifiedDate])
                        VALUES 
                        (EmployeeID
                       ,@NationalIDNumber
                       ,@EmployeeName
                       ,@LoginID
                       ,@JobTitle
                       ,@BirthDate
                       ,@MaritalStatus
                       ,@Gender
                       ,@HireDate
                       ,@VacationHours
                       ,@SickLeaveHours
                       ,@rowguid
                       ,@ModifiedDate)",
                        employee);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(AddEmployee)}: {ex.Message}", ex);
            }
        }

        public async Task<bool> CheckEmployee(int id)
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    return
                        (await connection.QueryFirstOrDefaultAsync<Employee>
                        ("SELECT * FROM Employee WITH (NOLOCK) WHERE EmployeeID = @EmployeeID", new { EmployeeID = id }) != null ? true : false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(CheckEmployee)}: {ex.Message}", ex);
            }
            return false;
        }

        public async Task DeleteEmployee(int id)
        {
            try
            {
                var employee = await GetEmployeeDetails(id);
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    await connection.QueryFirstOrDefaultAsync<Employee>("DELETE FROM Employee EmployeeID = @EmployeeID", new { EmployeeID = id });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(DeleteEmployee)}: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<Employee>> GetEmployeeDetails()
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    return await connection.QueryAsync<Employee>("SELECT * FROM Employee WITH (NOLOCK)");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetEmployeeDetails)}: {ex.Message}", ex);
            }
            return Enumerable.Empty<Employee>();
        }

        public async Task<Employee?> GetEmployeeDetails(int id)
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    return await connection.QueryFirstOrDefaultAsync<Employee>
                        ("SELECT * FROM Employee WITH (NOLOCK) WHERE EmployeeID = @EmployeeID", new { EmployeeID = id });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetEmployeeDetails)}: {ex.Message}", ex);
            }
            return null;
        }

        public async Task UpdateEmployee(Employee employee)
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    await connection.ExecuteAsync
                        (@"UPDATE [dbo].[Employee]
                       SET [EmployeeID] = @EmployeeID
                          ,[NationalIDNumber] = @NationalIDNumber
                          ,[EmployeeName] = @EmployeeName
                          ,[LoginID] = @LoginID
                          ,[JobTitle] = @JobTitle
                          ,[BirthDate] = @BirthDate
                          ,[MaritalStatus] = @MaritalStatus
                          ,[Gender] = @Gender
                          ,[HireDate] = @HireDate
                          ,[VacationHours] = @VacationHours
                          ,[SickLeaveHours] = @SickLeaveHours
                          ,[rowguid] = @rowguid
                          ,[ModifiedDate] = @ModifiedDate
                     WHERE EmployeeID = @EmployeeID",
                        employee);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(UpdateEmployee)}: {ex.Message}", ex);
            }
        }

        public async Task<UserInfo?> GetUserInfoAsync(string email, string password)
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    return await connection.QueryFirstOrDefaultAsync<UserInfo>
                        ("SELECT * FROM UserInfo WITH (NOLOCK) WHERE Email = @Email AND Password = @Password", new { Email = email, Password = password });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetUserInfoAsync)}: {ex.Message}", ex);
            }
            return null;
        }
    }
}
