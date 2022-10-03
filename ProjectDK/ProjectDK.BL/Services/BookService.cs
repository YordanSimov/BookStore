using ProjectDK.BL.Interfaces;
using ProjectDK.DL.Interfaces;
using ProjectDK.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDK.BL.Services
{
    public class BookService : IBookService
    {
        private readonly IBookInMemoryRepository bookInMemoryRepository;
        public BookService(IBookInMemoryRepository bookInMemoryRepository)
        {
            this.bookInMemoryRepository = bookInMemoryRepository;
        }
        public Book Add(Book book)
        {
           return bookInMemoryRepository.Add(book);
        }

        public Book? Delete(int id)
        {
            return bookInMemoryRepository.Delete(id);
        }

        public IEnumerable<Book> GetAll()
        {
            return bookInMemoryRepository.GetAll();
        }

        public Book? GetById(int id)
        {
            return bookInMemoryRepository.GetById(id);
        }

        public void Update(Book book)
        {
             bookInMemoryRepository.Update(book);
        }
    }
}
