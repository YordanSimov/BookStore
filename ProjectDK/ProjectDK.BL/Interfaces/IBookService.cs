using ProjectDK.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDK.BL.Interfaces
{
    public interface IBookService
    {
        IEnumerable<Book> GetAll();

        Book? GetById(int id);

        Book Add(Book input);

        void Update(Book input);

        Book? Delete(int id);
    }
}
