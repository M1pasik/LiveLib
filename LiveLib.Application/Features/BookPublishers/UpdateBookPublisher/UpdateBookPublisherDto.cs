
using LiveLib.Application.Common.Mapping;
using LiveLib.Domain.Models;

namespace LiveLib.Application.Features.BookPublishers.UpdateBookPublisher
{
    public class UpdateBookPublisherDto : IMapWith<BookPublisher>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
