using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProjectDK.DL.Interfaces;
using ProjectDK.Models.Models;
using System.Data.SqlClient;

namespace ProjectDK.DL.Repositories.MsSQL
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ILogger<AuthorRepository> _logger;
        private readonly IConfiguration _configuration;

        public AuthorRepository(ILogger<AuthorRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<Author> Add(Author input)
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    await connection.ExecuteAsync
                        (@"INSERT INTO [Authors] ([Name],Age,DateOfBirth,NickName) VALUES (@Name,@Age,@DateOfBirth,@NickName)",
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

        public async Task<bool> AddRange(IEnumerable<Author> authors)
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    await connection.ExecuteAsync
                    ("INSERT INTO [Authors] ([Name],Age,DateOfBirth,NickName) VALUES (@Name,@Age,@DateOfBirth,@NickName)", authors);

                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(AddRange)}: {ex.Message}", ex);
            }
            return false;
        }

        public async Task<Author?> Delete(int id)
        {
            try
            {
                var author = await GetById(id);
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    await connection.QueryFirstOrDefaultAsync<Author>("DELETE FROM Authors WHERE Id=@Id", new { Id = id });
                    return author;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(Delete)}: {ex.Message}", ex);
            }
            return null;
        }

        public async Task<IEnumerable<Author>> GetAll()
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    return await connection.QueryAsync<Author>("SELECT * FROM Authors WITH (NOLOCK)");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetAll)}: {ex.Message}", ex);
            }
            return Enumerable.Empty<Author>();
        }

        public async Task<Author?> GetById(int id)
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    return await connection.QueryFirstOrDefaultAsync<Author>("SELECT * FROM Authors WITH (NOLOCK) WHERE Id = @Id", new { Id = id });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetById)}: {ex.Message}", ex);
            }
            return null;
        }

        public async Task<Author?> GetByName(string name)
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    return await connection.QueryFirstOrDefaultAsync<Author>("SELECT * FROM Authors WITH (NOLOCK) WHERE [Name] = @Name", new { Name = name });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetByName)}: {ex.Message}", ex);
            }
            return null;
        }

        public async Task Update(Author input)
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    await connection.ExecuteAsync
                        ("UPDATE Authors SET [Name] = @Name,Age=@Age,DateOfBirth=@DateOfBirth,NickName=@Nickname WHERE Id = @Id",
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
