using AutoMapper;
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Interfaces;
using LiveLib.Application.Models.Collections;
using MediatR;

namespace LiveLib.Application.Features.Collections.UpdateCollection
{
    public class UpdateCollectionHandler : HandlerBase, IRequestHandler<UpdateCollectionCommand, Result<CollectionDto>>
    {
        public UpdateCollectionHandler(IMapper mapper, IDatabaseContext context) : base(mapper, context)
        {
        }

        public async Task<Result<CollectionDto>> Handle(UpdateCollectionCommand request, CancellationToken cancellationToken)
        {
            var collection = await _context.Collections.FindAsync(request.Id, cancellationToken);

            if (collection == null)
            {
                return Result<CollectionDto>.NotFound($"Collection {request.Id} not found");
            }

            _mapper.Map(request.Collection, collection);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(_mapper.Map<CollectionDto>(collection));

        }
    }
}
