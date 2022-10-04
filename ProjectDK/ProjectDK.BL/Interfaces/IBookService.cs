using ProjectDK.Models.Models;
using ProjectDK.Models.Requests;
using ProjectDK.Models.Responses;

namespace ProjectDK.BL.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAll();

        Task<Book?> GetById(int id);

        Task<BookResponse> Add(BookRequest input);

        Task<BookResponse> Update(BookRequest input);

        Task<Book?> Delete(int id);
    }
}
