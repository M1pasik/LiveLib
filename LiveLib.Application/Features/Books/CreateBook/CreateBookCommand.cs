using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Common.Mapping;
using LiveLib.Domain.Models;
using MediatR;

namespace LiveLib.Application.Features.Books.CreateBook
{
    public class CreateBookCommand : IRequest<Result<Guid>>, IMapWith<Book>
    {
        public string Name { get; set; } = null!;
        public DateOnly PublicatedAt { get; set; }
        public int PageCount { get; set; }
        public string Description { get; set; } = null!;
        public double AverageRating { get; set; }
        public string Isbn { get; set; } = null!;
        public Guid GenreId { get; set; }
        public Guid BookPublisherId { get; set; }
        public Guid AuthorId { get; set; }
    }
}
