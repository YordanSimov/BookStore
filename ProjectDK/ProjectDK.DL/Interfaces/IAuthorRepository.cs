using ProjectDK.Models.Models;

namespace ProjectDK.DL.Interfaces
{
    public interface IAuthorRepository : IRepository<Author>
    {
        Task<bool> AddRange(IEnumerable<Author> authors);

        Task<Author?> GetByName(string name);
    }
}
