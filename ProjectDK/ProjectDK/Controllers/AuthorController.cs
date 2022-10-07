using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectDK.BL.CommandHandlers;
using ProjectDK.Models.MediatR.Commands;
using ProjectDK.Models.Models;
using ProjectDK.Models.Requests;
using System.Net;

namespace ProjectDK.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class AuthorController : ControllerBase
    {
        private readonly ILogger<AuthorController> _logger;
        private readonly IMapper mapper;
        private readonly IMediator mediator;

        public AuthorController(
            ILogger<AuthorController> logger,
            IMapper mapper,
            IMediator mediator)
        {
            _logger = logger;
            this.mapper = mapper;
            this.mediator = mediator;
        }

        [HttpGet(nameof(GetAll))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetAll()
        {
            var authors = await mediator.Send(new GetAllAuthorsCommand());
            if (authors.Count() <= 0)
            {
                return NotFound("There aren't any authors in the collection");
            }
            _logger.LogInformation("Information test");
            _logger.LogWarning("Warning test");
            _logger.LogError("Error test");
            _logger.LogCritical("Critical test");
            return Ok(authors);
        }

        [HttpPost(nameof(Add))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> Add(AuthorRequest author)
        {
            var result = await mediator.Send(new AddAuthorCommand(author));

            try
            {
                if (result.HttpStatusCode == HttpStatusCode.BadRequest)
                    return BadRequest(author);
            }
            catch (Exception ex)
            {
                _logger.LogError("Bad request");
            }
            return Ok(result);
        }

        [HttpPost(nameof(AddAuthorsRange))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAuthorsRange([FromBody] AddAuthorsRequest addAuthors)
        {
            if (addAuthors == null || !addAuthors.Authors.Any())
                return BadRequest(addAuthors);

            var authors = mapper.Map<IEnumerable<Author>>(addAuthors.Authors);

            var result = await mediator.Send(new AddAuthorRangeCommand(authors));
            if (!result) return BadRequest(result);

            return Ok(result);
        }

        [HttpGet(nameof(GetById))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                if (id <= 0) throw new ArgumentOutOfRangeException("Id must be greater than 0");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            var result = await mediator.Send(new GetByIdAuthorCommand(id));

            if (result == null) return NotFound(id);

            return Ok(result);
        }

        [HttpPut(nameof(Update))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> Update(AuthorRequest author)
        {
            try
            {
                if (author == null) throw new NullReferenceException("Author can't be null");

            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("Author can't be null");
            }

            var result = await mediator.Send(new UpdateAuthorCommand(author));

            if (result.HttpStatusCode == HttpStatusCode.NotFound)
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete(nameof(Delete))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0) throw new ArgumentOutOfRangeException("Id should be greater than 0");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("Id must be greater than 0");
            }

            var result = await mediator.Send(new DeleteAuthorCommand(id));
            return result == null ? NotFound(id) : Ok(result);
        }
    }
}
