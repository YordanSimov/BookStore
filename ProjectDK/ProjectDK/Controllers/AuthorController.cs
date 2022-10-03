using Microsoft.AspNetCore.Mvc;
using ProjectDK.BL.Interfaces;
using ProjectDK.BL.Services;
using ProjectDK.Models.Models;
using ProjectDK.Models.Requests;
using System.Net;

namespace ProjectDK.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService authorService;
        private readonly ILogger<AuthorController> _logger;

        public AuthorController(IAuthorService authorService,
            ILogger<AuthorController> logger)
        {
            this.authorService = authorService;
            _logger = logger;
        }

        [HttpGet(nameof(GetAll))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult GetAll()
        {
            if (authorService.GetAll().Count() < 0)
            {
                return NotFound("There aren't any authors in the collection");
            }
            _logger.LogInformation("Information test");
            _logger.LogWarning("Warning test");
            _logger.LogError("Error test");
            _logger.LogCritical("Critical test");
            return Ok(authorService.GetAll());
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost(nameof(Add))]

        public IActionResult Add(AuthorRequest author)
        {
            //if (author == null) return BadRequest(author);

            //var authorExists = authorService.GetByName(author.Name);
            //if (authorExists != null) return BadRequest("Author already exists");
            // return Ok(authorService.Add(author));
            var result = authorService.Add(author);

            try
            {
                if (result.HttpStatusCode == HttpStatusCode.BadRequest)
                    throw new HttpRequestException("Bad request");
            }
            catch(Exception ex)
            {
                _logger.LogError("Bad request");
            }
            return Ok(result);
        }

        [HttpGet(nameof(GetById))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult GetById(int id)
        {
            try
            {
                if (id <= 0) throw new ArgumentOutOfRangeException("Id must be greater than 0");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            var result = authorService.GetById(id);

            if (result == null) return NotFound(id);

            return Ok(result);
        }

        [HttpPut(nameof(Update))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult Update(AuthorRequest author)
        {
            try
            {
                if (author == null) throw new NullReferenceException("Author can't be null");

            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(ex.Message);
            }

            var result = authorService.Update(author);

            if (result.HttpStatusCode == HttpStatusCode.NotFound)
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete(nameof(Delete))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Delete(int id)
        {
            try
            {
                if (id <= 0) throw new ArgumentOutOfRangeException("Id is smaller than 0");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogError(ex.Message);
            }

            var result = authorService.Delete(id);
            return result == null ? NotFound(id) : Ok(result);
        }
    }
}
