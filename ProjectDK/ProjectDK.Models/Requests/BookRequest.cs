namespace ProjectDK.Models.Requests
{
    public class BookRequest
    {
        public int Id { get; init; }

        public string Title { get; init; }

        public int AuthorId { get; init; }
    }
}
