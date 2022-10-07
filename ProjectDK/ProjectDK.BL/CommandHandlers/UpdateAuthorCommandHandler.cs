using AutoMapper;
using MediatR;
using ProjectDK.DL.Interfaces;
using ProjectDK.Models.MediatR.Commands;
using ProjectDK.Models.Models;
using ProjectDK.Models.Responses;
using System.Net;

namespace ProjectDK.BL.CommandHandlers
{
    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, UpdateAuthorResponse>
    {
        private readonly IAuthorRepository authorRepository;
        private readonly IMapper mapper;

        public UpdateAuthorCommandHandler(IAuthorRepository authorRepository, IMapper mapper)
        {
            this.authorRepository = authorRepository;
            this.mapper = mapper;
        }
        public async Task<UpdateAuthorResponse> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            var authorCheck = await authorRepository.GetById(request.authorRequest.Id);
            if (authorCheck == null)
            {
                return new UpdateAuthorResponse()
                {
                    HttpStatusCode = HttpStatusCode.NotFound,
                    Message = "Author to update does not exist"
                };
            }
            var author = mapper.Map<Author>(request.authorRequest);
            await authorRepository.Update(author);

            return new UpdateAuthorResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Message = "Successfully updated author."
            };
        }
    }
}
