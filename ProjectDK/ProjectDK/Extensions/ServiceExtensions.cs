using ProjectDK.BL.Dataflow;
using ProjectDK.BL.Interfaces;
using ProjectDK.BL.Kafka;
using ProjectDK.BL.Services;
using ProjectDK.Caches;
using ProjectDK.DL.Interfaces;
using ProjectDK.DL.MongoRepositories;
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
            services.AddSingleton<IPurchaseRepository, PurchaseRepository>();
            services.AddSingleton<IShoppingCartRepository, ShoppingCartRepository>();

            return services;
        }
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IPersonService, PersonService>();
            services.AddSingleton<IPurchaseService, PurchaseService>();
            services.AddSingleton<IEmployeeService, EmployeeService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddSingleton<IShoppingCartService, ShoppingCartService>();

            services.AddHostedService<PurchaseDataflow>();
            services.AddHostedService<DeliveryDataflow>();

            return services;
        }

        public static IServiceCollection RegisterCache<TKey, TValue>(this IServiceCollection services) where TValue : ICacheItem<TKey>
        {
            services.AddHostedService<KafkaCacheDistributor<TKey, TValue>>();
            services.AddSingleton<IKafkaConsumerCache<TKey, TValue>, KafkaConsumerCache<TKey, TValue>>();

            return services;
        }
    }
}
