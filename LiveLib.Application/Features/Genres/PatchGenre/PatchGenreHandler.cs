using AutoMapper;
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Interfaces;
using LiveLib.Application.Models.Genres;
using MediatR;

namespace LiveLib.Application.Features.Genres.PatchGenre
{
    public class PatchGenreHandler : HandlerBase, IRequestHandler<PatchGenreCommand, Result<GenreDetailDto>>
    {
        public PatchGenreHandler(IMapper mapper, IDatabaseContext context) : base(mapper, context)
        {
        }

        public async Task<Result<GenreDetailDto>> Handle(PatchGenreCommand request, CancellationToken cancellationToken)
        {
            var genre = await _context.Genres.FindAsync(request.Id, cancellationToken);
            if (genre == null)
            {
                return Result<GenreDetailDto>.NotFound($"Genre {request.Id} not found");
            }

            request.Patch.ApplyTo(genre);
            var updated = await _context.SaveChangesAsync(cancellationToken);

            return updated == 0 ? Result<GenreDetailDto>.ServerError("Genre not updated") : Result.Success(_mapper.Map<GenreDetailDto>(genre));
        }
    }
}
