using AutoMapper;
using MediatR;
using ProjectDK.BL.Interfaces;
using ProjectDK.DL.Interfaces;
using ProjectDK.DL.Repositories.MsSQL;
using ProjectDK.Models.MediatR.Commands;
using ProjectDK.Models.Models;
using ProjectDK.Models.Requests;
using ProjectDK.Models.Responses;
using System.Net;

namespace ProjectDK.BL.CommandHandlers
{
    public class AddBookCommandHandler : IRequestHandler<AddBookCommand, BookResponse>
    {
        private readonly IBookRepository bookRepository;
        private readonly IAuthorRepository authorRepository;
        private readonly IMapper mapper;

        public AddBookCommandHandler(IBookRepository bookRepository,IAuthorRepository authorRepository,IMapper mapper)
        {
            this.bookRepository = bookRepository;
            this.authorRepository = authorRepository;
            this.mapper = mapper;
        }
        public async Task<BookResponse> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            var bookCheck = await bookRepository.GetById(request.book.Id);
            var authorCheck = await authorRepository.GetById(request.book.AuthorId);
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

            var book = mapper.Map<Book>(request.book);
            var result = await bookRepository.Add(book);

            return new BookResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Book = result
            };
        }
    }
}
