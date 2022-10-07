using MediatR;
using ProjectDK.Models.Models;

namespace ProjectDK.Models.MediatR.Commands
{
    public record GetAllAuthorsCommand : IRequest<IEnumerable<Author>>
    {
    }
}
