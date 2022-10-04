using AutoMapper;
using ProjectDK.BL.Interfaces;
using ProjectDK.DL.Interfaces;
using ProjectDK.Models.Models;
using ProjectDK.Models.Requests;
using ProjectDK.Models.Responses;
using System.Net;

namespace ProjectDK.BL.Services
{
    public class AuthorService: IAuthorService
    {
        private readonly IAuthorRepository authorInMemoryRepository;
        private readonly IMapper mapper;
        private readonly IBookRepository bookRepository;


        public AuthorService(IAuthorRepository authorInMemoryRepository, IMapper mapper, IBookRepository bookRepository)
        {
            this.authorInMemoryRepository = authorInMemoryRepository;
            this.mapper = mapper;
            this.bookRepository = bookRepository;
        }
        public async Task<AddAuthorResponse> Add(AuthorRequest authorRequest)
        {
            var authorCheck = await authorInMemoryRepository.GetByName(authorRequest.Name);
            if (authorCheck != null)
            {
                return new AddAuthorResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "Author already exists",
                };
            }
            var author = mapper.Map<Author>(authorRequest);
            var result = await authorInMemoryRepository.Add(author);
            return new AddAuthorResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Author = result,
            };
        }

        public async Task<bool> AddRange(IEnumerable<Author> addAuthors)
        {
          return await authorInMemoryRepository.AddRange(addAuthors);
        }

        public async Task<Author?> Delete(int id)
        {
            if ((await bookRepository.GetAll()).Any(x=>x.AuthorId == id))
            {
                return null;
            }
            return await authorInMemoryRepository.Delete(id);
        }

        public async Task<IEnumerable<Author>> GetAll()
        {
            return await authorInMemoryRepository.GetAll();
        }

        public async Task<Author?> GetById(int id)
        {
            return await authorInMemoryRepository.GetById(id);
        }

        public async Task<Author?> GetByName(string name)
        {
            return await authorInMemoryRepository.GetByName(name);
        }

        public async Task<UpdateAuthorResponse> Update(AuthorRequest authorRequest)
        {
            var authorCheck = await authorInMemoryRepository.GetById(authorRequest.Id);
            if (authorCheck == null)
            {
                return new UpdateAuthorResponse()
                {
                    HttpStatusCode = HttpStatusCode.NotFound,
                    Message = "Author to update does not exist"
                };
            }
            var author = mapper.Map<Author>(authorRequest);
            await authorInMemoryRepository.Update(author);

            return new UpdateAuthorResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Message = "Successfully updated author."
            };
        }
    }
}
