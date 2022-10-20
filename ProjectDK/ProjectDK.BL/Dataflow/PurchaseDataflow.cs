using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Bson.IO;
using ProjectDK.BL.Kafka;
using ProjectDK.DL.Interfaces;
using ProjectDK.Models.Configurations;
using ProjectDK.Models.Models;
using System.Net.Http.Headers;
using System.Text;
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
            updateBlock = new TransformBlock<Purchase, IEnumerable<Book>>(async purchase =>
            {
                var additionalInfoText = new List<string>();
                HttpClient client = new HttpClient();

                    client.BaseAddress = new Uri("https://localhost:49157/AdditionalInfo/GetAdditionalInfoById?id=");
                foreach (var book in purchase.Books)
                {
                    var response = await client.GetAsync($"{client.BaseAddress}{book.AuthorId}");
                    response.EnsureSuccessStatusCode();

                    var additionalInfo = await response.Content.ReadAsStringAsync();

                    additionalInfoText.Add(additionalInfo);
                }
                purchase.AdditionalInfo = additionalInfoText;
              
                var result = await UpdateQuantity(purchase);

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
