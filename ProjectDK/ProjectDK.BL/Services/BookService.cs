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
        private readonly IBookRepository bookRepository;
        private readonly IAuthorRepository authorRepository;
        private readonly IMapper mapper;
        public BookService(IBookRepository bookInMemoryRepository, IMapper mapper, IAuthorRepository authorRepository)
        {
            this.bookRepository = bookInMemoryRepository;
            this.mapper = mapper;
            this.authorRepository = authorRepository;
        }
        public async Task<BookResponse> Add(BookRequest bookRequest)
        {
            var bookCheck = await bookRepository.GetById(bookRequest.Id);
            var authorCheck = await authorRepository.GetById(bookRequest.AuthorId);
            if (bookCheck != null) return new BookResponse()
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                Message = "Book already exists",
            };

            if (authorCheck == null)
            {
                return new BookResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "Author does not exist",
                };
            }

            var book = mapper.Map<Book>(bookRequest);
            var result = await bookRepository.Add(book);

           return new BookResponse()
           {
               HttpStatusCode = HttpStatusCode.OK,
               Book = result
           };
        }

        public async Task<Book?> Delete(int id)
        {
            return await bookRepository.Delete(id);
        }

        public async Task<IEnumerable<Book>> GetAll()
        {
            return await bookRepository.GetAll();
        }

        public async Task<Book?> GetById(int id)
        {
            return await bookRepository.GetById(id);
        }

        public async Task<BookResponse> Update(BookRequest bookRequest)
        {
            var bookCheck = await bookRepository.GetById(bookRequest.Id);
            if (bookCheck == null) return new BookResponse()
            {
                HttpStatusCode = HttpStatusCode.NotFound,
                Message = "Book does not exist.",
            };
            var book = mapper.Map<Book>(bookRequest);
            await bookRepository.Update(book);

            return new BookResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Book = book,
            };
        }
    }
}
