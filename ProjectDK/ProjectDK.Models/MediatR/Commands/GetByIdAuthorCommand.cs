using MediatR;
using ProjectDK.Models.Models;

namespace ProjectDK.BL.CommandHandlers
{
    public record GetByIdAuthorCommand(int id) : IRequest<Author?>
    {
    }
}
