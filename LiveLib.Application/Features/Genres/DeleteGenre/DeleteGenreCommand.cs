using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Models.Genres;
using MediatR;

namespace LiveLib.Application.Features.Genres.DeleteGenre
{
    public record DeleteGenreCommand(Guid Id) : IRequest<Result<GenreDetailDto>>;
}
