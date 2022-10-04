namespace ProjectDK.Models.Models
{
    public class Book
    {
        public int Id { get; init; }
        
        public string Title { get; init; }

        public int AuthorId { get; init; }

        public int Quantity { get; set; }

        public DateTime LastUpdated { get; set; }

        public decimal Price { get; set; }
    }
}
