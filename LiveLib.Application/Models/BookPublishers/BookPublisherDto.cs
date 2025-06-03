using AutoMapper;
using LiveLib.Application.Common.Mapping;
using LiveLib.Domain.Models;

namespace LiveLib.Application.Models.BookPublishers
{
    public class BookPublisherDto : IMapWith<BookPublisher>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<BookPublisher, BookPublisherDto>();
        }
    }
}
