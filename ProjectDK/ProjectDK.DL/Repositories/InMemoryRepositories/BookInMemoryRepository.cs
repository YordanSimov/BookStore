using ProjectDK.DL.Interfaces;
using ProjectDK.Models.Models;

namespace ProjectDK.DL.Repositories.InMemoryRepositories
{
    public class BookInMemoryRepository : IBookInMemoryRepository
    {
        private static List<Book> books = new List<Book>()
        {
            new Book(1,"1984",3),
            new Book(2,"Brave new world",2),
            new Book(3,"To kill a mockingbird",1),

        };
        public Book Add(Book book)
        {
            try
            {
                books.Add(book);
            }
            catch (Exception ex)
            {
                return null;
            }
            return book;
        }

        public Book? Delete(int bookId)
        {
            if (bookId <= 0)
            {
                return null;
            }
            var book = books.FirstOrDefault(x => x.Id == bookId);

            books.Remove(book);
            return book;
        }

        public IEnumerable<Book> GetAll()
        {
            return books;
        }

        public Book? GetById(int id)
        {
            return books.FirstOrDefault(x => x.Id == id);
        }

        public void Update(Book book)
        {
            var existingBook = books.FirstOrDefault(x => x.Id == book.Id);
            if (existingBook != null)
            {
                books.Remove(existingBook);
                books.Add(book);
            }
        }
    }
}
