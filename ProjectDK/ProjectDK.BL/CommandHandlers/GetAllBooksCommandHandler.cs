using MediatR;
using ProjectDK.Caches;
using ProjectDK.Models.MediatR.Commands;
using ProjectDK.Models.Models;

namespace ProjectDK.BL.CommandHandlers
{
    public class GetAllBooksCommandHandler : IRequestHandler<GetAllBooksCommand, IEnumerable<Book>>
    {
        private readonly IKafkaConsumerCache<int, Book> kafkaConsumer;

        public GetAllBooksCommandHandler(IKafkaConsumerCache<int, Book> kafkaConsumer)
        {
            this.kafkaConsumer = kafkaConsumer;
        }
        public async Task<IEnumerable<Book>> Handle(GetAllBooksCommand request, CancellationToken cancellationToken)
        {
            return await kafkaConsumer.GetAllAsync();
        }
    }
}
