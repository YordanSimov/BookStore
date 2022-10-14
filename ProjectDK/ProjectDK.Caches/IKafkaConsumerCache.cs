namespace ProjectDK.Caches
{
    public interface IKafkaConsumerCache<TKey, TValue>
    {
        List<TValue> CacheCollection { get; set; }

        void Consume(CancellationToken cancellationToken);

        Task<IEnumerable<TValue>> GetAllAsync();
    }
}