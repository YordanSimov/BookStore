using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProjectDK.DL.Interfaces;
using ProjectDK.Models.Configurations;
using ProjectDK.Models.Models;

namespace ProjectDK.DL.MongoRepositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly IOptionsMonitor<MongoDbSettings> options;
        private readonly ILogger<ShoppingCartRepository> logger;
        private readonly IPurchaseRepository purchaseRepository;
        private readonly MongoClient dbClient;
        private readonly IMongoDatabase database;
        private readonly IMongoCollection<ShoppingCart> collection;

        public ShoppingCartRepository(IOptionsMonitor<MongoDbSettings> options,
            ILogger<ShoppingCartRepository> logger,
            IPurchaseRepository purchaseRepository)
        {
            this.options = options;
            this.logger = logger;
            this.purchaseRepository = purchaseRepository;
            dbClient = new MongoClient(options.CurrentValue.ConnectionString);
            database = dbClient.GetDatabase(options.CurrentValue.DatabaseName);
            collection = database.GetCollection<ShoppingCart>(options.CurrentValue.CollectionShoppingCart);
        }
        public async Task AddToCart(ShoppingCart shoppingCart)
        {
            try
            {
                await collection.InsertOneAsync(shoppingCart);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }

        public async Task<ShoppingCart?> GetShoppingCart(int userId)
        {
            var data = await collection.FindAsync(x => x.UserId == userId);
            var shoppingCartCheck = data.ToEnumerable().FirstOrDefault();
            return shoppingCartCheck;
        }

        public async Task EmptyCart(int userId)
        {
            try
            {
                await collection.DeleteManyAsync(x => x.UserId == userId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }

        public async Task FinishPurchase(int userId)
        {
            try
            {
                var userCollection = await collection.FindAsync(x => x.UserId == userId);
                var userCart = userCollection.ToEnumerable().FirstOrDefault();
                var books = new List<Book>();

                foreach (var item in userCart.Books)
                {
                    books.Add(item);
                }

                var purchase = new Purchase()
                {
                    UserId = userCart.UserId,
                    Books = books,
                    TotalMoney = books.Select(x => x.Price).Sum(),
                };
                await purchaseRepository.SavePurchase(purchase);
                await EmptyCart(userId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }

        public async Task<IEnumerable<Book>> GetContent(int userId)
        {
            try
            {
                var data = await collection.FindAsync(x => x.UserId == userId);
                var books = data.ToList().FirstOrDefault();
                return books.Books;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return Enumerable.Empty<Book>();
            }
        }

        public async Task<ShoppingCart?> RemoveFromCart(ShoppingCart shoppingCart)
        {
            try
            {
                await collection.FindOneAndReplaceAsync(x => x.Id == shoppingCart.Id,shoppingCart);

                return shoppingCart;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
