using Microsoft.AspNetCore.Mvc;
using ProjectDK.BL.Interfaces;
using ProjectDK.BL.Services;
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
        public IEnumerable<Person> Get()
        {
            return personService.GetAll();
        }
        [HttpPost(nameof(Add))]
        public Person? Add(Person author)
        {
            return personService.Add(author);
        }

        [HttpGet(nameof(GetById))]
        public Person? GetById(int id)
        {
            return personService.GetById(id);
        }

        [HttpPut(nameof(Update))]
        public void Update(Person author)
        {
            personService.Update(author);
        }

        [HttpDelete(nameof(Delete))]
        public Person? Delete(int id)
        {
            return personService.Delete(id);
        }
    }
}