using ProjectDK.DL.Interfaces;
using ProjectDK.Models.Models;

namespace ProjectDK.DL.Repositories.InMemoryRepositories
{
    public class AuthorInMemoryRepository : IAuthorRepository
    {
        private static List<Author> authors = new List<Author>()
        {
            new Author(){Id = 1,Name = "Pesho", Age = 20, Nickname = "pesho123"},
            new Author(){Id = 2,Name = "Gosho", Age = 30, Nickname = "gosho123"},
            new Author(){Id = 3,Name = "Kocio", Age = 40, Nickname = "kocio123"}
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

            if (author != null)
            {
                authors.Remove(author);
            }
            return author;
        }

        public IEnumerable<Author> GetAll()
        {
            return authors;
        }

        public Author? GetByName(string name)
        {
            return authors.FirstOrDefault(x => x.Name == name);
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
