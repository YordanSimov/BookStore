using MediatR;
using ProjectDK.DL.Interfaces;
using ProjectDK.Models.MediatR.Commands;
using ProjectDK.Models.Models;

namespace ProjectDK.BL.CommandHandlers
{
    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, Author?>
    {
        private readonly IAuthorRepository authorRepository;
        private readonly IBookRepository bookRepository;

        public DeleteAuthorCommandHandler(IAuthorRepository authorRepository,IBookRepository bookRepository)
        {
            this.authorRepository = authorRepository;
            this.bookRepository = bookRepository;
        }
        public async Task<Author?> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            if ((await bookRepository.GetAll()).Any(x => x.AuthorId == request.id))
            {
                return null;
            }
            return await authorRepository.Delete(request.id);
        }
    }
}
