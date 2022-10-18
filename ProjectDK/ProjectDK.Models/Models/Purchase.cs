namespace ProjectDK.Models.Models
{
    public record Purchase
    {
        public Guid Id { get; set; }

        public IEnumerable<Book> Books { get; set; } = Enumerable.Empty<Book>();

        public decimal TotalMoney { get; set; }

        public int UserId { get; set; }
    }
}