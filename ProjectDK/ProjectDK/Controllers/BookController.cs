using Microsoft.AspNetCore.Mvc;
using ProjectDK.BL.Interfaces;
using ProjectDK.Models.Requests;
using System.Net;

namespace ProjectDK.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class BookController : ControllerBase
    {
        private readonly IBookService bookService;
        private readonly ILogger<AuthorController> _logger;

        public BookController(IBookService bookService,
            ILogger<AuthorController> logger)
        {
            this.bookService = bookService;
            _logger = logger;
        }

        [HttpGet(nameof(GetAll))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAll()
        {
            if (bookService.GetAll().Count() < 0)
            {
                return NotFound("There aren't any books in the collection");
            }
            return Ok(bookService.GetAll());
        }
        [HttpPost(nameof(Add))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Add(BookRequest book)
        {
            var result = bookService.Add(book);

            if (result.HttpStatusCode == HttpStatusCode.BadRequest)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet(nameof(GetById))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            if (id <= 0) return BadRequest("Id must be greater than 0");
            var result = bookService.GetById(id);

            if (result == null) return NotFound(id);

            return Ok(result);
        }

        [HttpPut(nameof(Update))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult Update(BookRequest book)
        {
            if (book == null) return BadRequest("Book can't be null");

            var result = bookService.Update(book);

            if (result.HttpStatusCode == HttpStatusCode.NotFound)
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete(nameof(Delete))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public IActionResult Delete(int id)
        {
            if (id <= 0) return BadRequest("Id must be greater than 0");

            var result = bookService.Delete(id);
            return result == null ? NotFound(id) : Ok(result);
        }
    }
}
