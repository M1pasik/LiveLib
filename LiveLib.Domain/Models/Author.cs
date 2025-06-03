namespace LiveLib.Domain.Models
{
    public class Author
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string SecondName { get; set; } = string.Empty;

        public string ThirdName { get; set; } = string.Empty;

        public string Biography { get; set; } = string.Empty;

        public List<Book> Books { get; set; } = [];
    }
}
