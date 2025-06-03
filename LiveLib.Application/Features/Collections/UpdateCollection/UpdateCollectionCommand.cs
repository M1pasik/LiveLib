using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Models.Collections;
using MediatR;

namespace LiveLib.Application.Features.Collections.UpdateCollection
{
    public record UpdateCollectionCommand(Guid Id, UpdateCollectionDto Collection) : IRequest<Result<CollectionDto>>;
}
