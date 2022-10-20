using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ProjectDK.BL.Kafka;
using ProjectDK.DL.Interfaces;
using ProjectDK.Models.Configurations;
using ProjectDK.Models.Models;
using System.Threading.Tasks.Dataflow;

namespace ProjectDK.BL.Dataflow
{
    public class PurchaseDataflow : IHostedService
    {
        private readonly IBookRepository bookRepository;
        private readonly KafkaConsumerService<Guid, Purchase> kafkaConsumerService;
        private readonly IOptionsMonitor<KafkaConsumerSettings> kafkaConsumerSettings;

        private TransformBlock<Purchase, IEnumerable<Book>> updateBlock;
        private ActionBlock<IEnumerable<Book>> addBlock;

        public PurchaseDataflow(IBookRepository bookRepository,
            IOptionsMonitor<KafkaConsumerSettings> kafkaConsumerSettings)
        {
            this.bookRepository = bookRepository;
            this.kafkaConsumerService = new KafkaConsumerService<Guid, Purchase>(kafkaConsumerSettings, HandlePurchase);
            this.kafkaConsumerSettings = kafkaConsumerSettings;
            updateBlock = new TransformBlock<Purchase,IEnumerable<Book>>(async x =>
            {
               var result = await UpdateQuantity(x);
                return result;
            });
            addBlock = new ActionBlock<IEnumerable<Book>>(async result =>
            {
                if (result.Any())
                {
                    foreach (var book in result)
                    {
                        await bookRepository.Add(book);
                    }
                }
            });
            updateBlock.LinkTo(addBlock);
        }

        private void HandlePurchase(Purchase purchase)
        {
            updateBlock.Post(purchase);
        }

        public async Task<IEnumerable<Book>> UpdateQuantity(Purchase purchase)
        {
            var purchasedBooks = purchase.Books.ToList();
            var notExistingBooks = new List<Book>();
            foreach (var book in purchasedBooks)
            {
                var bookDB = await bookRepository.GetById(book.Id);
                if (bookDB == null)
                {
                    notExistingBooks.Add(book);
                    continue;
                }
                bookDB.Quantity -= 1;
                await bookRepository.Update(bookDB);
            }
            return notExistingBooks;
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
    }
}
