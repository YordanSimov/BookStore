namespace ProjectDK.Models.Requests
{
    public class BookRequest
    {
        public int Id { get; init; }

        public string Title { get; init; }

        public int AuthorId { get; init; }

        public DateTime LastUpdated { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
