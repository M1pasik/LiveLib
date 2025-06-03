using LiveLib.Application.Models.Genres;
using MediatR;

namespace LiveLib.Application.Features.Genres.GetGenres
{
    public class GetGenresQuery : IRequest<ICollection<GenreDto>>;
}
