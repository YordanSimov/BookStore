using MediatR;
using ProjectDK.Models.Models;

namespace ProjectDK.Models.MediatR.Commands
{
    public record GetAllBooksCommand : IRequest<IEnumerable<Book>>
    {

    }
}
