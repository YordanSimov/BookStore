using MediatR;
using ProjectDK.Models.Models;

namespace ProjectDK.Models.MediatR.Commands
{
    public record DeleteAuthorCommand(int id) : IRequest<Author?>
    {
    }
}
