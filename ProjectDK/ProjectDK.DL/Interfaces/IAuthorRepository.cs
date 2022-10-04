using ProjectDK.Models.Models;

namespace ProjectDK.DL.Interfaces
{
    public interface IAuthorRepository : IRepository<Author>
    {
        bool AddRange(IEnumerable<Author> authors);
        Author? GetByName(string name);
    }
}
