namespace LiveLib.Domain.Models
{
    public class BookPublisher
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public List<Book> Books { get; set; } = [];
    }
}
