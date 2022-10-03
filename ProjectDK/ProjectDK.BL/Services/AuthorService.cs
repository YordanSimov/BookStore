using ProjectDK.BL.Interfaces;
using ProjectDK.DL.Interfaces;
using ProjectDK.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDK.BL.Services
{
    public class AuthorService: IAuthorService
    {
        private readonly IAuthorInMemoryRepository authorInMemoryRepository;

        public AuthorService(IAuthorInMemoryRepository authorInMemoryRepository)
        {
            this.authorInMemoryRepository = authorInMemoryRepository;
        }
        public Author Add(Author author)
        {
            return authorInMemoryRepository.Add(author);
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

        public void Update(Author author)
        {
            authorInMemoryRepository.Update(author);
        }
    }
}
