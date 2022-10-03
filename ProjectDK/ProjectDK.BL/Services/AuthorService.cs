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

        public AuthorService(IAuthorRepository authorInMemoryRepository, IMapper mapper)
        {
            this.authorInMemoryRepository = authorInMemoryRepository;
            this.mapper = mapper;
        }
        public AddAuthorResponse Add(AuthorRequest authorRequest)
        {
            var authorCheck = authorInMemoryRepository.GetByName(authorRequest.Name);
            if (authorCheck != null)
            {
                return new AddAuthorResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "Author already exists",
                };
            }
            var author = mapper.Map<Author>(authorRequest);
            var result = authorInMemoryRepository.Add(author);
            return new AddAuthorResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Author = result
            };
        }

        public Author? Delete(int id)
        {
            return authorInMemoryRepository.Delete(id);
        }

        public IEnumerable<Author> GetAll()
        {
            return authorInMemoryRepository.GetAll();
        }

        public Author? GetById(int id)
        {
            return authorInMemoryRepository.GetById(id);
        }

        public Author? GetByName(string name)
        {
            return authorInMemoryRepository.GetByName(name);
        }

        public UpdateAuthorResponse Update(AuthorRequest authorRequest)
        {
            var authorCheck = authorInMemoryRepository.GetById(authorRequest.Id);
            if (authorCheck == null)
            {
                return new UpdateAuthorResponse()
                {
                    HttpStatusCode = HttpStatusCode.NotFound,
                    Message = "Author to update does not exist"
                };
            }
            var author = mapper.Map<Author>(authorRequest);
            authorInMemoryRepository.Update(author);

            return new UpdateAuthorResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Message = "Successfully updated author."
            };
        }
    }
}
