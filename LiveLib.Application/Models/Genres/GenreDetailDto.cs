using LiveLib.Application.Common.Mapping;
using LiveLib.Application.Models.Books;
using LiveLib.Domain.Models;

namespace LiveLib.Application.Models.Genres
{
    public class GenreDetailDto : IMapWith<Genre>
    {
        public string Name { get; set; } = string.Empty;

        public int AgeRestriction { get; set; }

        public List<BookDto> Books { get; set; } = [];

        //public void Mapping(Profile profile)
        //{
        //    profile.CreateMap<Genre, GenreDetailDto>()
        //        .ForMember(dest => dest.Books, opt => opt.MapFrom(src => src.Books));
        //}
    }
}
