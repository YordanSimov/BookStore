using ProjectDK.Models.Models;

namespace ProjectDK.BL.Interfaces
{
    public interface IPersonService
    {
        Task<IEnumerable<Person>> GetAll();

        Task<Person?> GetById(int id);

        Task<Person> Add(Person person);

        Task Update(Person person);

        Task<Person?> Delete(int id);
    }
}
