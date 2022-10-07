using AutoMapper;
using MediatR;
using ProjectDK.DL.Interfaces;
using ProjectDK.Models.MediatR.Commands;
using ProjectDK.Models.Models;
using ProjectDK.Models.Responses;
using System.Net;

namespace ProjectDK.BL.CommandHandlers
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, BookResponse>
    {
        private readonly IBookRepository bookRepository;
        private readonly IMapper mapper;

        public UpdateBookCommandHandler(IBookRepository bookRepository,IMapper mapper)
        {
            this.bookRepository = bookRepository;
            this.mapper = mapper;
        }
        public async Task<BookResponse> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var bookCheck = await bookRepository.GetById(request.book.Id);
            if (bookCheck == null) return new BookResponse()
            {
                HttpStatusCode = HttpStatusCode.NotFound,
                Message = "Book does not exist.",
            };
            var book = mapper.Map<Book>(request.book);
            await bookRepository.Update(book);

            return new BookResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Book = book,
            };
        }
    }
}
