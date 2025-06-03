namespace LiveLib.Domain.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateOnly PublicatedAt { get; set; }
        public int PageCount { get; set; }
        public string Description { get; set; } = string.Empty;
        public double AverageRating { get; set; }
        public string Isbn { get; set; } = string.Empty;
        public string CoverId { get; set; } = string.Empty;
        public List<Collection> Collections { get; set; } = [];

        public Guid GenreId { get; set; }
        public Genre Genre { get; set; } = null!;

        public Guid AuthorId { get; set; }
        public Author Author { get; set; } = null!;

        public Guid BookPublisherId { get; set; }
        public BookPublisher BookPublisher { get; set; } = null!;


    }
}
