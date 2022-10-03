using Microsoft.AspNetCore.Mvc;
using ProjectDK.BL.Interfaces;
using ProjectDK.Models.Models;

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
        public IEnumerable<Book> GetAll()
        {
            return bookService.GetAll();
        }
        [HttpPost(nameof(Add))]
        public Book? Add(Book author)
        {
            return bookService.Add(author);
        }

        [HttpGet(nameof(GetById))]
        public Book? GetById(int id)
        {
            return bookService.GetById(id);
        }

        [HttpPut(nameof(Update))]
        public void Update(Book author)
        {
            bookService.Update(author);
        }

        [HttpDelete(nameof(Delete))]
        public Book? Delete(int id)
        {
            return bookService.Delete(id);
        }
    }
}
