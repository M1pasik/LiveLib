namespace LiveLib.Domain.Models
{
    public class Collection
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public User OwnerUser { get; set; } = null!;

        public Guid OwnerUserId { get; set; }

        public List<User> UsersSubscribers { get; set; } = [];

        public List<Book> Books { get; set; } = [];
    }
}
