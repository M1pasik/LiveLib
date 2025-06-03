using AutoMapper;
using AutoMapper.QueryableExtensions;
using LiveLib.Application.Interfaces;
using LiveLib.Application.Models.Genres;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiveLib.Application.Features.Genres.GetGenres
{
    public class GetGenresHandler : HandlerBase, IRequestHandler<GetGenresQuery, ICollection<GenreDto>>
    {
        public GetGenresHandler(IMapper mapper, IDatabaseContext context) : base(mapper, context)
        {
        }

        public async Task<ICollection<GenreDto>> Handle(GetGenresQuery request, CancellationToken cancellationToken)
        {
            return await _context.Genres.AsNoTracking()
                .ProjectTo<GenreDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
