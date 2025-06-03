using AutoMapper;
using LiveLib.Application.Common.Mapping;
using LiveLib.Application.Common.Mapping;
using LiveLib.Domain.Models;

namespace LiveLib.Application.Models.Genres
{
    public class GenreDto : IMapWith<Genre>
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int AgeRestriction { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Genre, GenreDto>();
        }
    }
}
