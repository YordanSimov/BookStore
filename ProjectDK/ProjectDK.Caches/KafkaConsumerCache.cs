using Confluent.Kafka;
using Microsoft.Extensions.Options;
using ProjectDK.Models.Configurations;

namespace ProjectDK.Caches
{
    public class KafkaConsumerCache<TKey, TValue> : IKafkaConsumerCache<TKey, TValue>
    {
        private readonly ConsumerConfig config;
        private readonly IConsumer<TKey, TValue> consumer;
        private readonly IOptionsMonitor<KafkaConsumerSettings> kafkaConsumerSettings;

        public KafkaConsumerCache(IOptionsMonitor<KafkaConsumerSettings> kafkaConsumerSettings)
        {
            this.kafkaConsumerSettings = kafkaConsumerSettings;
            config = new ConsumerConfig()
            {
                BootstrapServers = kafkaConsumerSettings.CurrentValue.BootstrapServers,
                GroupId = kafkaConsumerSettings.CurrentValue.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            consumer = new ConsumerBuilder<TKey, TValue>(config)
                .SetKeyDeserializer(new KafkaCacheDeserializer<TKey>())
                .SetValueDeserializer(new KafkaCacheDeserializer<TValue>()).Build();
            CacheCollection = new List<TValue>();
        }

        public List<TValue> CacheCollection { get; set; }

        public void Consume(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                consumer.Subscribe($"{typeof(TValue).Name}Cache");
                var cr = consumer.Consume(cancellationToken);
                CacheCollection.Add(cr.Value);

                Console.WriteLine($"Receiver msg with key:{cr.Message.Key} value:{cr.Message.Value}");
            }
        }

        public async Task<IEnumerable<TValue>> GetAllAsync()
        {
            return CacheCollection;
        }
    }
}