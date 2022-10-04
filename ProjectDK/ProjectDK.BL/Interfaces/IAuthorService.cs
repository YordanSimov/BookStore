using ProjectDK.Models.Models;
using ProjectDK.Models.Requests;
using ProjectDK.Models.Responses;

namespace ProjectDK.BL.Interfaces
{
    public interface IAuthorService
    {
        IEnumerable<Author> GetAll();

        Author? GetById(int id);

        Author? GetByName(string name);

        AddAuthorResponse Add(AuthorRequest input);

        UpdateAuthorResponse Update(AuthorRequest input);

        Author? Delete(int id);

        bool AddRange(IEnumerable<Author> addAuthors);
    }
}
