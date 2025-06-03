using LiveLib.Application.Common.Mapping;
using LiveLib.Domain.Models;

namespace LiveLib.Application.Features.Books.UpdateBook
{
    public class UpdateBookDto : IMapWith<Book>
    {
        public string? Name { get; set; }
        public DateOnly? PublicatedAt { get; set; }
        public int? PageCount { get; set; }
        public string? Description { get; set; }
        public double? AverageRating { get; set; }
        public string? Isbn { get; set; }

        public Guid? GenreId { get; set; }

        public Guid? AuthorId { get; set; }

        public Guid? BookPublisherId { get; set; }
    }
}
