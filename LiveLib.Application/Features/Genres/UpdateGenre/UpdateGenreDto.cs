using LiveLib.Application.Common.Mapping;
using LiveLib.Domain.Models;

namespace LiveLib.Application.Features.Genres.UpdateGenre
{
    public class UpdateGenreDto : IMapWith<Genre>
    {
        public string? Name { get; set; }

        public int? AgeRestriction { get; set; }
    }
}
