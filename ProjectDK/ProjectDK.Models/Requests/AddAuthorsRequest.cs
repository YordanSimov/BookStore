using ProjectDK.Models.Models;

namespace ProjectDK.Models.Requests
{
    public class AddAuthorsRequest
    {
        public IEnumerable<Author> Authors { get; set; }

        public string Reason { get; set; }
    }
}
