using Microsoft.AspNetCore.Mvc;
using ProjectDK.BL.Interfaces;
using ProjectDK.Models.Requests;
using System.Net;

namespace ProjectDK.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService personService;
        private readonly ILogger<PersonController> _logger;

        public PersonController(ILogger<PersonController> logger, IPersonService personService)
        {
            _logger = logger;
            this.personService = personService;
        }

        [HttpGet(nameof(GetAll))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            if ((await personService.GetAll()).Count() < 0)
            {
                return NotFound("There aren't any people in the collection");
            }
            return Ok(await personService.GetAll());
        }
        [HttpPost(nameof(Add))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add(PersonRequest person)
        {
            var result = await personService.Add(person);

            if (result.HttpStatusCode == HttpStatusCode.BadRequest)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet(nameof(GetById))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0) return BadRequest("Id must be greater than 0");
            var result = await personService.GetById(id);

            if (result == null) return NotFound(id);

            return Ok(result);
        }

        [HttpPut(nameof(Update))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(PersonRequest person)
        {
            if (person == null) return BadRequest("Person can't be null");

            var result = await personService.Update(person);

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

            var result = await personService.Delete(id);
            return result == null ? NotFound(id) : Ok(result);
        }
    }
}