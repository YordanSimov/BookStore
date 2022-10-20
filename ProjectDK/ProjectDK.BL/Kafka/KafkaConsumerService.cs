using Confluent.Kafka;
using Microsoft.Extensions.Options;
using ProjectDK.Models.Configurations;

namespace ProjectDK.BL.Kafka
{
    public class KafkaConsumerService<TKey, TValue> : IKafkaConsumerService<TKey, TValue>
    {
        private readonly ConsumerConfig config;
        private readonly IOptionsMonitor<KafkaConsumerSettings> kafkaConsumerSettings;
        private readonly IConsumer<TKey, TValue> consumer;
        private Action<TValue> action;

        public KafkaConsumerService(IOptionsMonitor<KafkaConsumerSettings> kafkaConsumerSettings, Action<TValue> action)
        {
            this.kafkaConsumerSettings = kafkaConsumerSettings;
            this.action = action;
            config = new ConsumerConfig()
            {
                BootstrapServers = kafkaConsumerSettings.CurrentValue.BootstrapServers,
                GroupId = kafkaConsumerSettings.CurrentValue.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
};
            consumer = new ConsumerBuilder<TKey, TValue>(config)
                .SetKeyDeserializer(new KafkaDeserializer<TKey>())
                .SetValueDeserializer(new KafkaDeserializer<TValue>()).Build();
        }
        public void Consume()
        {
            consumer.Subscribe($"{typeof(TValue).Name}Topic");
            while (true)
            {
                var cr = consumer.Consume();
                action.Invoke(cr.Message.Value);
            }
        }
    }
}
