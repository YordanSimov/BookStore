using MediatR;
using ProjectDK.DL.Interfaces;
using ProjectDK.Models.MediatR.Commands;
using ProjectDK.Models.Models;

namespace ProjectDK.BL.CommandHandlers
{
    public class GetAllAuthorsCommandHandler : IRequestHandler<GetAllAuthorsCommand, IEnumerable<Author>>
    {
        private readonly IAuthorRepository authorRepository;

        public GetAllAuthorsCommandHandler(IAuthorRepository authorRepository)
        {
            this.authorRepository = authorRepository;
        }
        public async Task<IEnumerable<Author>> Handle(GetAllAuthorsCommand request, CancellationToken cancellationToken)
        {
            return await authorRepository.GetAll();
        }
    }
}
