using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Models.Genres;
using LiveLib.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace LiveLib.Application.Features.Genres.PatchGenre
{
    public record PatchGenreCommand(Guid Id, JsonPatchDocument<Genre> Patch) : IRequest<Result<GenreDetailDto>>;
}
