using AutoMapper;
using LiveLib.Application.Common.Mapping;
using LiveLib.Application.Models.Books;
using LiveLib.Domain.Models;

namespace LiveLib.Application.Models.BookPublishers
{
    public class BookPublisherDetailDto : IMapWith<BookPublisher>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<BookDto> Books { get; set; } = [];

        public void Mapping(Profile profile)
        {
            profile.CreateMap<BookPublisher, BookPublisherDetailDto>()
                .ForMember(dest => dest.Books, opt => opt.MapFrom(src => src.Books));
        }
    }
}
