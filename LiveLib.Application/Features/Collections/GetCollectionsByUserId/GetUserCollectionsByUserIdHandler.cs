using AutoMapper;
using AutoMapper.QueryableExtensions;
using LiveLib.Application.Interfaces;
using LiveLib.Application.Models.Collections;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiveLib.Application.Features.Collections.GetCollectionsByUserId
{
    public class GetUserCollectionsByUserIdHandler : HandlerBase, IRequestHandler<GetUserCollectionsByUserIdQuery, ICollection<CollectionDto>>
    {
        public GetUserCollectionsByUserIdHandler(IMapper mapper, IDatabaseContext context) : base(mapper, context)
        {
        }

        public async Task<ICollection<CollectionDto>> Handle(GetUserCollectionsByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Collections
                .AsNoTracking()
                .Where(c => c.OwnerUserId == request.UserId)
                .ProjectTo<CollectionDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
