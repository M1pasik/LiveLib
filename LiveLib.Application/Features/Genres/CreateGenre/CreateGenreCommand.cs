using LiveLib.Application.Common.Mapping;
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Domain.Models;
using MediatR;

namespace LiveLib.Application.Features.Genres.CreateGenre
{
    public class CreateGenreCommand : IRequest<Result<Guid>>, IMapWith<Genre>
    {
        public string Name { get; set; } = null!;
        public int AgeRestriction { get; set; }
    }
}
