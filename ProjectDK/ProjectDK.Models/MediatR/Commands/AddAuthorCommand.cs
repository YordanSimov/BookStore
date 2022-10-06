using MediatR;
using ProjectDK.Models.Requests;
using ProjectDK.Models.Responses;

namespace ProjectDK.Models.MediatR.Commands
{
    public record AddAuthorCommand(AuthorRequest author) : IRequest<AddAuthorResponse>
    {
    }
}
