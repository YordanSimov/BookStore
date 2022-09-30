using ProjectDK.BL.Interfaces;
using ProjectDK.DL.Interfaces;
using ProjectDK.Models.Models;

namespace ProjectDK.BL.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonInMemoryRepository personInMemoryRepository;

        public PersonService(IPersonInMemoryRepository personInMemoryRepository)
        {
            this.personInMemoryRepository = personInMemoryRepository;
        }
        public Person Add(Person person)
        {
            return personInMemoryRepository.Add(person);
        }

        public Person? Delete(int id)
        {
            return personInMemoryRepository.Delete(id);
        }

        public IEnumerable<Person> GetAll()
        {
            return personInMemoryRepository.GetAll();
        }

        public Person? GetById(int id)
        {
            return personInMemoryRepository.GetById(id);
        }

        public void Update(Person person)
        {
            personInMemoryRepository.Update(person);
        }
    }
}