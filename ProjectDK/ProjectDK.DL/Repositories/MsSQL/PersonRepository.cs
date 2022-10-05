using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProjectDK.DL.Interfaces;
using ProjectDK.Models.Models;
using System.Data.SqlClient;

namespace ProjectDK.DL.Repositories.MsSQL
{
    public class PersonRepository : IPersonRepository
    {

        private readonly ILogger<PersonRepository> _logger;
        private readonly IConfiguration _configuration;

        public PersonRepository(ILogger<PersonRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<Person> Add(Person input)
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    await connection.ExecuteAsync
                        (@"INSERT INTO Person (Id,Name,Age,DateOfBirth) VALUES (@Id,@Name,@Age,@DateOfBirth)",
                        input);
                    return input;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(Add)}: {ex.Message}", ex);
            }
            return null;
        }

        public async Task<Person?> Delete(int id)
        {
            try
            {
                var person = await GetById(id);
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    await connection.QueryFirstOrDefaultAsync<Author>("DELETE FROM Person WHERE Id=@Id", new { Id = id });
                    return person;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(Delete)}: {ex.Message}", ex);
            }
            return null;
        }

        public async Task<IEnumerable<Person>> GetAll()
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    return await connection.QueryAsync<Person>("SELECT * FROM Person WITH (NOLOCK)");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetAll)}: {ex.Message}", ex);
            }
            return Enumerable.Empty<Person>();
        }

        public async Task<Person?> GetById(int id)
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    return await connection.QueryFirstOrDefaultAsync<Person>("SELECT * FROM Person WITH (NOLOCK) WHERE Id = @Id", new { Id = id });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetById)}: {ex.Message}", ex);
            }
            return null;
        }

        public async Task Update(Person input)
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    await connection.ExecuteAsync
                        ("UPDATE Person SET Name = @Name,Age = @Age,DateOfBirth=@DateOfBirth WHERE Id = @Id",
                        input);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(Update)}: {ex.Message}", ex);
            }
        }
    }
}
