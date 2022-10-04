using ProjectDK.DL.Interfaces;
using ProjectDK.Models.Models;

namespace ProjectDK.DL.Repositories.MsSQL
{
    public class PersonRepository : IPersonRepository
    {
        public Task<Person> Add(Person input)
        {
            throw new NotImplementedException();
        }

        public Task<Person?> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Person>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Person?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Person input)
        {
            throw new NotImplementedException();
        }
    }
}
