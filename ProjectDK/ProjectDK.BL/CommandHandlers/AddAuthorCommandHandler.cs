using AutoMapper;
using MediatR;
using ProjectDK.DL.Interfaces;
using ProjectDK.Models.MediatR.Commands;
using ProjectDK.Models.Models;
using ProjectDK.Models.Responses;
using System.Net;

namespace ProjectDK.BL.CommandHandlers
{
    public class AddAuthorCommandHandler : IRequestHandler<AddAuthorCommand, AddAuthorResponse>
    {
        private readonly IAuthorRepository authorRepository;
        private readonly IMapper mapper;

        public AddAuthorCommandHandler(IAuthorRepository authorRepository, IMapper mapper)
        {
            this.authorRepository = authorRepository;
            this.mapper = mapper;
        }
        public async Task<AddAuthorResponse> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
        {
            var authorCheck = await authorRepository.GetByName(request.author.Name);
            if (authorCheck != null)
            {
                return new AddAuthorResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "Author already exists",
                };
            }
            var author = mapper.Map<Author>(request.author);
            var result = await authorRepository.Add(author);
            return new AddAuthorResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Author = result,
            };
        }
    }
}
