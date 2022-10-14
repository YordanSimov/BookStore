using Microsoft.Extensions.Hosting;
using ProjectDK.Models.Models;

namespace ProjectDK.Caches
{
    public class KafkaCacheDistributor<TKey,TValue> : IHostedService where TValue : ICacheItem<TKey>
    {
        private readonly IKafkaConsumerCache<TKey, TValue> kafkaConsumer;

        public KafkaCacheDistributor(IKafkaConsumerCache<TKey,TValue> kafkaConsumer)
        {
            this.kafkaConsumer = kafkaConsumer;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() => kafkaConsumer.Consume(cancellationToken));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
