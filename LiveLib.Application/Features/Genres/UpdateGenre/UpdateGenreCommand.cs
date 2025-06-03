using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Models.Genres;
using MediatR;

namespace LiveLib.Application.Features.Genres.UpdateGenre
{
    public record UpdateGenreCommand(Guid Id, UpdateGenreDto Genre) : IRequest<Result<GenreDetailDto>>;
}
