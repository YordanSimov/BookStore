using MediatR;
using ProjectDK.Models.Models;

namespace ProjectDK.Models.MediatR.Commands
{
    public record AddAuthorRangeCommand(IEnumerable<Author> authors) : IRequest<bool>
    {
    }
}
