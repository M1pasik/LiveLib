using AutoMapper;
using AutoMapper.QueryableExtensions;
using LiveLib.Application.Interfaces;
using LiveLib.Application.Models.Authors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiveLib.Application.Features.Authors.GetAuthors
{
    public class GetAuthorsHandler : HandlerBase, IRequestHandler<GetAuthorsQuery, ICollection<AuthorDto>>
    {
        public GetAuthorsHandler(IMapper mapper, IDatabaseContext context) : base(mapper, context)
        {
        }

        public async Task<ICollection<AuthorDto>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Authors.AsNoTracking()
                .ProjectTo<AuthorDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
