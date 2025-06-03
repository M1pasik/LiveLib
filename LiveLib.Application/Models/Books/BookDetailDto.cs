using AutoMapper;
using LiveLib.Application.Common.Mapping;
using LiveLib.Application.Models.Authors;
using LiveLib.Application.Models.BookPublishers;
using LiveLib.Application.Models.Genres;
using LiveLib.Domain.Models;

namespace LiveLib.Application.Models.Books
{
    public class BookDetailDto : IMapWith<Book>
    {
        public string Name { get; set; } = string.Empty;
        public DateOnly PublicatedAt { get; set; }
        public int PageCount { get; set; }
        public string Description { get; set; } = string.Empty;
        public double AverageRating { get; set; }
        public string Isbn { get; set; } = string.Empty;
        public string CoverId { get; set; } = string.Empty;

        public GenreDto Genre { get; set; } = null!;

        public AuthorDto Author { get; set; } = null!;

        public BookPublisherDto BookPublisher { get; set; } = null!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Book, BookDetailDto>()
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre));
        }
    }
}
