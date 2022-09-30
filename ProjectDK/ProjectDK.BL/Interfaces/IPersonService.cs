using ProjectDK.Models.Models;

namespace ProjectDK.BL.Interfaces
{
    public interface IPersonService
    {
        IEnumerable<Person> GetAll();

        Person? GetById(int id);

        Person Add(Person person);

        void Update(Person person);

        Person? Delete(int id);
    }
}
