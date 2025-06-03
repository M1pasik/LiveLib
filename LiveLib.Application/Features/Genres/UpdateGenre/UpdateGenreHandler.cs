using AutoMapper;
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Interfaces;
using LiveLib.Application.Models.Genres;
using MediatR;

namespace LiveLib.Application.Features.Genres.UpdateGenre
{
    public class UpdateGenreHandler : HandlerBase, IRequestHandler<UpdateGenreCommand, Result<GenreDetailDto>>
    {
        public UpdateGenreHandler(IMapper mapper, IDatabaseContext context) : base(mapper, context)
        {
        }

        public async Task<Result<GenreDetailDto>> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
        {
            var genre = await _context.Genres.FindAsync(request.Id, cancellationToken);

            if (genre == null)
            {
                return Result<GenreDetailDto>.NotFound($"Genre {request.Id} not found");
            }

            _mapper.Map(request.Genre, genre);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(_mapper.Map<GenreDetailDto>(genre));
        }
    }
}
