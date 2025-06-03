using AutoMapper;
using LiveLib.Application.Common.Mapping;
using LiveLib.Domain.Models;

namespace LiveLib.Application.Models.Collections
{
    public class CollectionDto : IMapWith<Collection>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Collection, CollectionDto>();
        }
    }
}
