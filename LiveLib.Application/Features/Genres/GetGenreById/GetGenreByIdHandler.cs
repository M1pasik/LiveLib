using AutoMapper;
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Interfaces;
using LiveLib.Application.Models.Genres;
using MediatR;

namespace LiveLib.Application.Features.Genres.GetGenreById
{
    public class GetGenreByIdHandler : HandlerBase, IRequestHandler<GetGenreByIdQuery, Result<GenreDetailDto>>
    {
        public GetGenreByIdHandler(IMapper mapper, IDatabaseContext context) : base(mapper, context)
        {
        }

        public async Task<Result<GenreDetailDto>> Handle(GetGenreByIdQuery request, CancellationToken cancellationToken)
        {
            var genre = await _context.Genres.FindAsync(request.Id, cancellationToken);

            return genre is null ? Result<GenreDetailDto>.NotFound($"Genre {request.Id} not found") :
                Result.Success(_mapper.Map<GenreDetailDto>(genre));
        }
    }
}
