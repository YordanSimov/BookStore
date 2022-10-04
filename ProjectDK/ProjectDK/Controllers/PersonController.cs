using Microsoft.AspNetCore.Mvc;
using ProjectDK.BL.Interfaces;
using ProjectDK.Models.Models;

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

        [HttpGet("Get")]
        public async Task<IEnumerable<Person>> Get()
        {
            return await personService.GetAll();
        }
        [HttpPost(nameof(Add))]
        public async Task<Person?> Add(Person author)
        {
            return await personService.Add(author);
        }

        [HttpGet(nameof(GetById))]
        public async Task<Person?> GetById(int id)
        {
            return await personService.GetById(id);
        }

        [HttpPut(nameof(Update))]
        public async Task Update(Person author)
        {
            await personService.Update(author);
        }

        [HttpDelete(nameof(Delete))]
        public async Task<Person?> Delete(int id)
        {
            return await personService.Delete(id);
        }
    }
}