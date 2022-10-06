using MediatR;
using ProjectDK.DL.Interfaces;
using ProjectDK.Models.MediatR.Commands;

namespace ProjectDK.BL.CommandHandlers
{
    public class AddAuthorsCommandHandler : IRequestHandler<AddAuthorRangeCommand, bool>
    {
        private readonly IAuthorRepository authorRepository;

        public AddAuthorsCommandHandler(IAuthorRepository authorRepository)
        {
            this.authorRepository = authorRepository;
        }

        public async Task<bool> Handle(AddAuthorRangeCommand request, CancellationToken cancellationToken)
        {
            return await authorRepository.AddRange(request.authors);
        }
    }
}
