namespace ProjectDK.Models.Configurations
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string CollectionPurchase { get; set; }

        public string CollectionShoppingCart { get; set; }
    }
}