using ProjectDK.Models.Models;
using ProjectDK.Models.Requests;
using ProjectDK.Models.Responses;

namespace ProjectDK.BL.Interfaces
{
    public interface IAuthorService
    {
        Task<IEnumerable<Author>> GetAll();

        Task<Author?> GetById(int id);

        Task<Author?> GetByName(string name);

        Task<AddAuthorResponse> Add(AuthorRequest input);

        Task<UpdateAuthorResponse> Update(AuthorRequest input);

        Task<Author?> Delete(int id);

        Task<bool> AddRange(IEnumerable<Author> addAuthors);
    }
}
