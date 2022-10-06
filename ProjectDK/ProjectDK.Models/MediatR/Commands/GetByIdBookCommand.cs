using MediatR;
using ProjectDK.Models.Models;

namespace ProjectDK.Models.MediatR.Commands
{
    public record GetByIdBookCommand(int bookId) : IRequest<Book?>
    {
    }
}
