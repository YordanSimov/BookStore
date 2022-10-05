using ProjectDK.Models.Models;
using ProjectDK.Models.Requests;
using ProjectDK.Models.Responses;

namespace ProjectDK.BL.Interfaces
{
    public interface IPersonService
    {
        Task<IEnumerable<Person>> GetAll();

        Task<Person?> GetById(int id);

        Task<PersonResponse> Add(PersonRequest person);

        Task<PersonResponse> Update(PersonRequest person);

        Task<Person> Delete(int id);
    }
}
