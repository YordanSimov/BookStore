using MediatR;
using ProjectDK.DL.Interfaces;
using ProjectDK.Models.MediatR.Commands;
using ProjectDK.Models.Models;

namespace ProjectDK.BL.CommandHandlers
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, Book?>
    {
        private readonly IBookRepository bookRepository;

        public DeleteBookCommandHandler(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }
        public async Task<Book?> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            return await bookRepository.Delete(request.bookId);
        }
    }
}
