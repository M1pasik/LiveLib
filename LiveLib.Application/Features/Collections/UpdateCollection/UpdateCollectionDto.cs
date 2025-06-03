using LiveLib.Application.Common.Mapping;
using LiveLib.Domain.Models;

namespace LiveLib.Application.Features.Collections.UpdateCollection
{
    public class UpdateCollectionDto : IMapWith<Collection>
    {
        public string? Title { get; set; }

    }
}
