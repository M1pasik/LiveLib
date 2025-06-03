namespace LiveLib.Domain.Models
{
    public class Genre
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int AgeRestriction { get; set; }

        public List<Book> Books { get; set; } = [];
    }
}
