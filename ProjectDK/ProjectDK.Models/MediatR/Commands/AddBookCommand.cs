using MediatR;
using ProjectDK.Models.Requests;
using ProjectDK.Models.Responses;

namespace ProjectDK.Models.MediatR.Commands
{
    public record AddBookCommand(BookRequest book) : IRequest<BookResponse>
    {
    }
}
