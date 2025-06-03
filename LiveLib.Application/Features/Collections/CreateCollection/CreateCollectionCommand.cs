using LiveLib.Application.Common.Mapping;
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Domain.Models;
using MediatR;

namespace LiveLib.Application.Features.Collections.CreateCollection
{
    public class CreateCollectionCommand : IRequest<Result<Guid>>, IMapWith<Collection>
    {
        public string Title { get; set; } = string.Empty;
        public Guid OwnerUserId { get; set; }
    }
}
