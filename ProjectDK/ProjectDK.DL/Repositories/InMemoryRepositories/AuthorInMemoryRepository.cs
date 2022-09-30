using ProjectDK.DL.Interfaces;
using ProjectDK.Models.Models;

namespace ProjectDK.DL.Repositories.InMemoryRepositories
{
    public class AuthorInMemoryRepository : IAuthorInMemoryRepository
    {
        private static List<Author> authors = new List<Author>()
        {
            new Author(1,"Pesho",20,"pesho123"),
            new Author(2,"Gosho",30,"gosho123"),
            new Author(3,"Kocio",40,"kocio123"),

        };
        public Author Add(Author author)
        {
            try
            {
                authors.Add(author);
            }
            catch (Exception ex)
            {
                return null;
            }
            return author;
        }

        public Author? Delete(int authorId)
        {
            if (authorId <= 0)
            {
                return null;
            }
            var author = authors.FirstOrDefault(x => x.Id == authorId);

            authors.Remove(author);
            return author;
        }

        public IEnumerable<Author> GetAll()
        {
            return authors;
        }

        public Author? GetById(int id)
        {
            return authors.FirstOrDefault(x => x.Id == id);
        }

        public void Update(Author author)
        {
            var existingAuthor = authors.FirstOrDefault(x => x.Id == author.Id);
            if (existingAuthor != null)
            {
                authors.Remove(existingAuthor);
                authors.Add(author);
            }
        }
    }
}
