using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Models.Collections;
using MediatR;

namespace LiveLib.Application.Features.Collections.DeleteCollection
{
    public record DeleteCollectionCommand(Guid Id) : IRequest<Result<CollectionDto>>;
}
