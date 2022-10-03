using ProjectDK.DL.Interfaces;
using ProjectDK.Models.Models;

namespace ProjectDK.DL.Repositories.InMemoryRepositories
{
    public class PersonInMemoryRepository : IPersonRepository
    {
        private static List<Person> users = new List<Person>()
        {
            new Person(){Id = 1,Name = "Pesho",Age = 20},
            new Person(){Id = 2,Name = "Gosho",Age = 30},
            new Person(){Id = 3,Name = "Kocio",Age = 40 },
        };

        public IEnumerable<Person> GetAll()
        {
            return users;
        }

        public Person? GetById(int id)
        {
            return users.FirstOrDefault(x => x.Id == id);
        }

        public Person Add(Person user)
        {
            try
            {
                users.Add(user);
            }
            catch (Exception ex)
            {
                return null;
            }
            return user;
        }

        public void Update(Person user)
        {
            var existingUser = users.FirstOrDefault(x => x.Id == user.Id);
            if (existingUser != null)
            {
                users.Remove(existingUser);
                users.Add(user);
            }
        }

        public Person? Delete(int userId)
        {
            if (userId <= 0)
            {
                return null;
            }
            var user = users.FirstOrDefault(x => x.Id == userId);

            if (user != null)
            {
                users.Remove(user);
            }
            return user;
        }
    }
}