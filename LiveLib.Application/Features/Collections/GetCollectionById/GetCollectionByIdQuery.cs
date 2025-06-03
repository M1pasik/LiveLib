using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Models.Collections;
using MediatR;

namespace LiveLib.Application.Features.Collections.GetCollectionById
{
    public record GetCollectionByIdQuery(Guid Id) : IRequest<Result<CollectionDetailDto>>;
}
