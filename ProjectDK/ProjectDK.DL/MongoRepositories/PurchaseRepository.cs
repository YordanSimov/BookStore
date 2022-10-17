using DnsClient.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProjectDK.DL.Interfaces;
using ProjectDK.Models.Configurations;
using ProjectDK.Models.Models;

namespace ProjectDK.DL.MongoRepositories
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly IOptionsMonitor<MongoDbSettings> options;
        private readonly ILogger<PurchaseRepository> logger;
        private readonly MongoClient dbClient;
        private readonly IMongoDatabase database;
        private readonly IMongoCollection<Purchase> collection;

        public PurchaseRepository(IOptionsMonitor<MongoDbSettings> options, ILogger<PurchaseRepository> logger)
        {
            this.options = options;
            this.logger = logger;
            dbClient = new MongoClient(options.CurrentValue.ConnectionString);
            database = dbClient.GetDatabase(options.CurrentValue.DatabaseName);
            collection = database.GetCollection<Purchase>(options.CurrentValue.CollectionPurchase);
        }
        public async Task<Guid> DeletePurchase(Purchase purchase)
        {
            try
            {
                var deleted = await collection.FindOneAndDeleteAsync(x => x.Id == purchase.Id);
                return deleted.Id;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return Guid.Empty;
            }
        }

        public async Task<IEnumerable<Purchase>> GetPurchases(int userId)
        {
            try
            {
                var result = await collection.FindAsync(x => x.UserId == userId);
                var a= result.ToEnumerable();
                return a;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<Purchase?> SavePurchase(Purchase purchase)
        {
            try
            {
               await collection.InsertOneAsync(purchase);
                return purchase;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }
    }
}