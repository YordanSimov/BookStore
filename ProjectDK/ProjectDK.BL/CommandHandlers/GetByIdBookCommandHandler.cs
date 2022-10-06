using MediatR;
using ProjectDK.DL.Interfaces;
using ProjectDK.Models.MediatR.Commands;
using ProjectDK.Models.Models;

namespace ProjectDK.BL.CommandHandlers
{
    public class GetByIdBookCommandHandler : IRequestHandler<GetByIdBookCommand,Book?>
    {
        private readonly IBookRepository bookRepository;

        public GetByIdBookCommandHandler(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        public async Task<Book?> Handle(GetByIdBookCommand request, CancellationToken cancellationToken)
        {
            return await bookRepository.GetById(request.bookId);
        }
    }
}
