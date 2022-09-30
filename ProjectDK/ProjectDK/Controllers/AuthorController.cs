using Microsoft.AspNetCore.Mvc;
using ProjectDK.BL.Interfaces;
using ProjectDK.Models.Models;

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
        public IEnumerable<Author> GetAll()
        {
            return authorService.GetAll();
        }

        [HttpPost(nameof(Add))]
        public Author? Add(Author author)
        {
            return authorService.Add(author);
        }

        [HttpGet(nameof(GetById))]
        public Author? GetById(int id)
        {
            return authorService.GetById(id);
        }

        [HttpPut(nameof(Update))]
        public void Update(Author author)
        {
            authorService.Update(author);
        }

        [HttpDelete(nameof(Delete))]
        public Author? Delete(int id)
        {
            return authorService.Delete(id);
        }
    }
}
