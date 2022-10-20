using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ProjectDK.BL.Kafka;
using ProjectDK.DL.Interfaces;
using ProjectDK.Models.Configurations;
using ProjectDK.Models.Models;
using System.Threading.Tasks.Dataflow;

namespace ProjectDK.BL.Dataflow
{
    public class DeliveryDataflow : IHostedService
    {
        private readonly IBookRepository bookRepository;
        private readonly KafkaConsumerService<int, Delivery> kafkaConsumerService;
        private readonly IOptionsMonitor<KafkaConsumerSettings> kafkaConsumerSettings;

        private TransformBlock<Delivery, Book> updateBlock;
        private ActionBlock<Book> addBlock;

        public DeliveryDataflow(IBookRepository bookRepository,
            IOptionsMonitor<KafkaConsumerSettings> kafkaConsumerSettings)
        {
            this.bookRepository = bookRepository;
            this.kafkaConsumerService = new KafkaConsumerService<int, Delivery>(kafkaConsumerSettings, HandleDelivery);
            this.kafkaConsumerSettings = kafkaConsumerSettings;
            updateBlock = new TransformBlock<Delivery, Book>(async x =>
            {
                var result = await UpdateQuantity(x);
                return result;
            });
            addBlock = new ActionBlock<Book>(result =>
            {
                if (result != null)
                {
                    bookRepository.Add(result);
                }
            });
            updateBlock.LinkTo(addBlock);
        }

        private void HandleDelivery(Delivery delivery)
        {
            updateBlock.Post(delivery);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() => 
            kafkaConsumerService.Consume());
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task<Book> UpdateQuantity(Delivery delivery)
        {
            var book = await bookRepository.GetById(delivery.Book.Id);
            if (book == null)
            {
                return delivery.Book;
            }
            book.Quantity += 1;
            await bookRepository.Update(book);
            return null;
        }
    }
}