using MediatR;
using ProjectDK.Models.Models;

namespace ProjectDK.Models.MediatR.Commands
{
    public record DeleteBookCommand(int bookId) : IRequest<Book?>
    {
    }
}
