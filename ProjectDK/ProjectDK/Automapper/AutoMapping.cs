using AutoMapper;
using ProjectDK.Models.Models;
using ProjectDK.Models.Requests;

namespace ProjectDK.Automapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<AuthorRequest, Author>();
            CreateMap<BookRequest, Book>();
            CreateMap<PersonRequest, Person>();
        }
    }
}
