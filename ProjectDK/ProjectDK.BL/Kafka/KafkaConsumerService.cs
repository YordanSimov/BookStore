using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ProjectDK.Models.Configurations;

namespace ProjectDK.BL.Kafka
{
    public class KafkaConsumerService<TKey, TValue> : IKafkaConsumerService<TKey, TValue>,IHostedService
    {
        private readonly ConsumerConfig config;
        private readonly IConsumer<TKey, TValue> consumer;
        public IOptionsMonitor<KafkaConsumerSettings> kafkaConsumerSettings { get; set; }

        public KafkaConsumerService(IOptionsMonitor<KafkaConsumerSettings> kafkaConsumerSettings)
        {
            this.kafkaConsumerSettings = kafkaConsumerSettings;
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
            consumer.Subscribe("test2");
            while (true)
            {
                var cr = consumer.Consume();

                Console.WriteLine($"Receiver msg with key:{cr.Message.Key} value:{cr.Message.Value}");

            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() => Consume());
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
