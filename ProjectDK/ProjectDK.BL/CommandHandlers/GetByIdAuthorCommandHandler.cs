using MediatR;
using ProjectDK.DL.Interfaces;
using ProjectDK.Models.Models;

namespace ProjectDK.BL.CommandHandlers
{
    public class GetByIdAuthorCommandHandler : IRequestHandler<GetByIdAuthorCommand, Author?>
    {
        private readonly IAuthorRepository authorRepository;

        public GetByIdAuthorCommandHandler(IAuthorRepository authorRepository)
        {
            this.authorRepository = authorRepository;
        }
        public async Task<Author?> Handle(GetByIdAuthorCommand request, CancellationToken cancellationToken)
        {
            return await authorRepository.GetById(request.id);
        }
    }
}
