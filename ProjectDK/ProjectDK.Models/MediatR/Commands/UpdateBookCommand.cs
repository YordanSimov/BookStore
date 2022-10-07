using MediatR;
using ProjectDK.Models.Requests;
using ProjectDK.Models.Responses;

namespace ProjectDK.Models.MediatR.Commands
{
    public record UpdateBookCommand(BookRequest book) : IRequest<BookResponse>
    {
    }
}
