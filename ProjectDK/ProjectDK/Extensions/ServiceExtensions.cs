using ProjectDK.BL.Interfaces;
using ProjectDK.BL.Services;
using ProjectDK.DL.Interfaces;
using ProjectDK.DL.Repositories.MsSQL;

namespace ProjectDK.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorRepository, AuthorRepository>();
            services.AddSingleton<IBookRepository, BookRepository>();
            services.AddSingleton<IPersonRepository, PersonRepository>();
            services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
          //  services.AddSingleton<IUserInfoRepository, UserInfoStore>();
            return services;
        }
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IPersonService, PersonService>();
            services.AddSingleton<IEmployeeService, EmployeeService>();
            return services;
        }
    }
}
