using AutoMapper;
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Interfaces;
using LiveLib.Domain.Models;
using MediatR;

namespace LiveLib.Application.Features.Collections.CreateCollection
{
    public class CreateCollectionHandler : HandlerBase, IRequestHandler<CreateCollectionCommand, Result<Guid>>
    {
        public CreateCollectionHandler(IMapper mapper, IDatabaseContext context) : base(mapper, context)
        {
        }

        public async Task<Result<Guid>> Handle(CreateCollectionCommand request, CancellationToken cancellationToken)
        {
            var collection = _mapper.Map<Collection>(request);
            _context.Collections.Add(collection);
            var saved = await _context.SaveChangesAsync(cancellationToken);

            return saved == 0 ? Result<Guid>.ServerError("Collection not saved") : Result.Success(collection.Id);
        }
    }
}
