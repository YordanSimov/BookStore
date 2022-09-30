using ProjectDK.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDK.BL.Interfaces
{
    public interface IAuthorService
    {
        IEnumerable<Author> GetAll();

        Author? GetById(int id);

        Author Add(Author input);

        void Update(Author input);

        Author? Delete(int id);
    }
}
