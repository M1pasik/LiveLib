using AutoMapper;
using AutoMapper.QueryableExtensions;
using LiveLib.Application.Interfaces;
using LiveLib.Application.Models.BookPublishers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiveLib.Application.Features.BookPublishers.GetBookPublishers
{
    public class GetBookPublishersHandler : HandlerBase, IRequestHandler<GetBookPublishersQuery, ICollection<BookPublisherDto>>
    {
        public GetBookPublishersHandler(IMapper mapper, IDatabaseContext context) : base(mapper, context)
        {
        }

        public async Task<ICollection<BookPublisherDto>> Handle(GetBookPublishersQuery request, CancellationToken cancellationToken)
        {
            return await _context.BookPublishers.AsNoTracking()
                            .ProjectTo<BookPublisherDto>(_mapper.ConfigurationProvider)
                            .ToListAsync(cancellationToken);
        }
    }
}
