using AutoMapper;
using LiveLib.Application.Common.Mapping;
using LiveLib.Application.Models.Books;
using LiveLib.Application.Models.Users;
using LiveLib.Domain.Models;

namespace LiveLib.Application.Models.Collections
{
    public class CollectionDetailDto : IMapWith<Collection>
    {
        public string Title { get; set; } = string.Empty;

        public List<UserDto> Users { get; set; } = [];

        public List<BookDto> Books { get; set; } = [];

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Collection, CollectionDetailDto>()
                .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.UsersSubscribers))
                .ForMember(dest => dest.Books, opt => opt.MapFrom(src => src.Books));
        }
    }
}
