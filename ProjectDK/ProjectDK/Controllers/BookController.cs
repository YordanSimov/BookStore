using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectDK.Models.MediatR.Commands;
using ProjectDK.Models.Requests;
using System.Net;

namespace ProjectDK.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class BookController : ControllerBase
    {
        private readonly ILogger<AuthorController> _logger;
        private readonly IMediator mediator;

        public BookController(
            ILogger<AuthorController> logger,
            IMediator mediator)
        {
            _logger = logger;
            this.mediator = mediator;
        }

        [HttpGet(nameof(GetAll))]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            var books = await mediator.Send(new GetAllBooksCommand());
            if (books.Count() <= 0)
            {
                return NotFound("There aren't any books in the collection");
            }
            return Ok(books);
        }
        [HttpPost(nameof(Add))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add(BookRequest book)
        {
            var result = await mediator.Send(new AddBookCommand(book));

            if (result.HttpStatusCode == HttpStatusCode.BadRequest)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet(nameof(GetById))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "User")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0) return BadRequest("Id must be greater than 0");
            var result = await mediator.Send(new GetByIdBookCommand(id));

            if (result == null) return NotFound(id);

            return Ok(result);
        }

        [HttpPut(nameof(Update))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> Update(BookRequest book)
        {
            if (book == null) return BadRequest("Book can't be null");

            var result = await mediator.Send(new UpdateBookCommand(book));

            if (result.HttpStatusCode == HttpStatusCode.NotFound)
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete(nameof(Delete))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest("Id must be greater than 0");

            var result = await mediator.Send(new DeleteBookCommand(id));
            return result == null ? NotFound(id) : Ok(result);
        }
    }
}
