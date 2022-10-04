using ProjectDK.BL.Interfaces;
using ProjectDK.DL.Interfaces;
using ProjectDK.Models.Models;

namespace ProjectDK.BL.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository personInMemoryRepository;

        public PersonService(IPersonRepository personInMemoryRepository)
        {
            this.personInMemoryRepository = personInMemoryRepository;
        }
        public async Task<Person> Add(Person person)
        {
            return await personInMemoryRepository.Add(person);
        }

        public async Task<Person?> Delete(int id)
        {
            return await personInMemoryRepository.Delete(id);
        }

        public async Task<IEnumerable<Person>> GetAll()
        {
            return await personInMemoryRepository.GetAll();
        }

        public async Task<Person?> GetById(int id)
        {
            return await personInMemoryRepository.GetById(id);
        }

        public async Task Update(Person person)
        {
            await personInMemoryRepository.Update(person);
        }
    }
}