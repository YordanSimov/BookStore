using ProjectDK.Models.Models;

namespace ProjectDK.DL.Interfaces
{
    public interface IAuthorRepository : IRepository<Author>
    {
        Author? GetByName(string name);
    }
}
