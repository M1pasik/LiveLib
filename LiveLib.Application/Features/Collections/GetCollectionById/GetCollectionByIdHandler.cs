using AutoMapper;
using AutoMapper.QueryableExtensions;
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Interfaces;
using LiveLib.Application.Models.Collections;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiveLib.Application.Features.Collections.GetCollectionById
{
    public class GetCollectionByIdHandler : HandlerBase, IRequestHandler<GetCollectionByIdQuery, Result<CollectionDetailDto>>
    {
        public GetCollectionByIdHandler(IMapper mapper, IDatabaseContext context) : base(mapper, context)
        {
        }

        public async Task<Result<CollectionDetailDto>> Handle(GetCollectionByIdQuery request, CancellationToken cancellationToken)
        {
            var collection = await _context.Collections
                .Where(g => g.Id == request.Id)
                .ProjectTo<CollectionDetailDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            return collection is null ? Result<CollectionDetailDto>.NotFound($"Collection {request.Id} not found") :
                Result.Success(_mapper.Map<CollectionDetailDto>(collection));
        }
    }
}
