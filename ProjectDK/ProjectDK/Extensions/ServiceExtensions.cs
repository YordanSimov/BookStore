using ProjectDK.BL.Interfaces;
using ProjectDK.BL.Services;
using ProjectDK.Caches;
using ProjectDK.DL.Interfaces;
using ProjectDK.DL.Repositories.MsSQL;
using ProjectDK.Models.Models;

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

            return services;
        }
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IPersonService, PersonService>();
            services.AddSingleton<IEmployeeService, EmployeeService>();
            services.AddTransient<IIdentityService, IdentityService>();

            return services;
        }

        public static IServiceCollection RegisterHostedService<TKey,TValue>(this IServiceCollection services) where TValue : ICacheItem<TKey>
        {
            services.AddHostedService<KafkaCacheDistributor<TKey, TValue>>();
            services.AddSingleton<IKafkaConsumerCache<TKey, TValue>, KafkaConsumerCache<TKey, TValue>>();

            return services;
        }
    }
}
