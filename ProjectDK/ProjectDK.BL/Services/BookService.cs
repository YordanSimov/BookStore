using AutoMapper;
using ProjectDK.BL.Interfaces;
using ProjectDK.DL.Interfaces;
using ProjectDK.Models.Models;
using ProjectDK.Models.Requests;
using ProjectDK.Models.Responses;
using System.Net;

namespace ProjectDK.BL.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository bookInMemoryRepository;
        private readonly IMapper mapper;
        public BookService(IBookRepository bookInMemoryRepository, IMapper mapper)
        {
            this.bookInMemoryRepository = bookInMemoryRepository;
            this.mapper = mapper;
        }
        public BookResponse Add(BookRequest bookRequest)
        {
            var bookCheck = bookInMemoryRepository.GetById(bookRequest.Id);
            if (bookCheck != null) return new BookResponse()
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                Message = "Book already exists",
            };

            var book = mapper.Map<Book>(bookRequest);
            var result = bookInMemoryRepository.Add(book);

           return new BookResponse()
           {
               HttpStatusCode = HttpStatusCode.OK,
               Book = result
           };
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

        public BookResponse Update(BookRequest bookRequest)
        {
            var bookCheck = bookInMemoryRepository.GetById(bookRequest.Id);
            if (bookCheck == null) return new BookResponse()
            {
                HttpStatusCode = HttpStatusCode.NotFound,
                Message = "Book does not exist.",
            };
            var book = mapper.Map<Book>(bookRequest);
            bookInMemoryRepository.Update(book);

            return new BookResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Book = book,
            };
        }
    }
}
