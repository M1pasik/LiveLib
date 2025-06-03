using AutoMapper;
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Interfaces;
using LiveLib.Application.Models.Genres;
using MediatR;

namespace LiveLib.Application.Features.Genres.DeleteGenre
{
    public class DeleteGenreHandler : HandlerBase, IRequestHandler<DeleteGenreCommand, Result<GenreDetailDto>>
    {
        public DeleteGenreHandler(IMapper mapper, IDatabaseContext context) : base(mapper, context)
        {
        }

        public async Task<Result<GenreDetailDto>> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
        {
            var genre = await _context.Genres.FindAsync(request.Id, cancellationToken);
            if (genre == null)
            {
                return Result<GenreDetailDto>.NotFound($"Genre {request.Id} not found");
            }

            _context.Genres.Remove(genre);
            var removed = await _context.SaveChangesAsync(cancellationToken);

            return removed == 0 ? Result<GenreDetailDto>.NotFound($"Genre {request.Id} not deleted") : Result<GenreDetailDto>.NoContent();
        }
    }
}
