namespace ProjectDK.Models.Models
{
    public class Book
    {
        public Book(int id, string title, int authorId)
        {
            Id = id;
            Title = title;
            AuthorId = authorId;
        }

        public int Id { get; init; }
        
        public string Title { get; init; }

        public int AuthorId { get; init; }
    }
}
