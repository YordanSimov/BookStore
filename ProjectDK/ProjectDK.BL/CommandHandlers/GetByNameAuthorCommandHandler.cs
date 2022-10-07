using MediatR;
using ProjectDK.DL.Interfaces;
using ProjectDK.Models.MediatR.Commands;
using ProjectDK.Models.Models;

namespace ProjectDK.BL.CommandHandlers
{
    public class GetByNameAuthorCommandHandler : IRequestHandler<GetByNameAuthorCommand, Author?>
    {
        private readonly IAuthorRepository authorRepository;

        public GetByNameAuthorCommandHandler(IAuthorRepository authorRepository)
        {
            this.authorRepository = authorRepository;
        }
        public async Task<Author?> Handle(GetByNameAuthorCommand request, CancellationToken cancellationToken)
        {
            return await authorRepository.GetByName(request.name);
        }
    }
}
