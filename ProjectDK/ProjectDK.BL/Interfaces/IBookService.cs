using ProjectDK.Models.Models;
using ProjectDK.Models.Requests;
using ProjectDK.Models.Responses;

namespace ProjectDK.BL.Interfaces
{
    public interface IBookService
    {
        IEnumerable<Book> GetAll();

        Book? GetById(int id);

        BookResponse Add(BookRequest input);

        BookResponse Update(BookRequest input);

        Book? Delete(int id);
    }
}
