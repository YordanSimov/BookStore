using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProjectDK.DL.Interfaces;
using ProjectDK.Models.Models;
using System.Data.SqlClient;

namespace ProjectDK.DL.Repositories.MsSQL
{
    public class BookRepository : IBookRepository
    {
        private readonly ILogger<BookRepository> _logger;
        private readonly IConfiguration _configuration;

        public BookRepository(ILogger<BookRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<Book> Add(Book input)
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    await connection.ExecuteAsync
                        (@"INSERT INTO [Books] (Title,AuthorId,Quantity,LastUpdated,Price) VALUES (@Title,@AuthorId,@Quantity,@LastUpdated,@Price)",
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

        public async Task<Book?> Delete(int id)
        {
            try
            {
                var book = await GetById(id);
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    await connection.QueryFirstOrDefaultAsync<Author>("DELETE FROM Books WHERE Id=@Id", new { Id = id });
                    return book;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(Delete)}: {ex.Message}", ex);
            }
            return null;
        }

        public async Task<IEnumerable<Book>> GetAll()
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    return await connection.QueryAsync<Book>("SELECT * FROM Books WITH (NOLOCK)");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetAll)}: {ex.Message}", ex);
            }
            return Enumerable.Empty<Book>();
        }

        public async Task<Book?> GetById(int id)
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    return await connection.QueryFirstOrDefaultAsync<Book>("SELECT * FROM Books WITH (NOLOCK) WHERE Id = @Id", new { Id = id });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetById)}: {ex.Message}", ex);
            }
            return null;
        }

        public async Task Update(Book input)
        {
            try
            {
                await using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    await connection.ExecuteAsync
                        ("UPDATE Books SET Title = @Title,AuthorId = @AuthorId,LastUpdated=@LastUpdated,Quantity=@Quantity,Price=@Price WHERE Id = @Id",
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
