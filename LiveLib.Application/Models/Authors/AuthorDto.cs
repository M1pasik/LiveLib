using AutoMapper;
using LiveLib.Application.Common.Mapping;
using LiveLib.Domain.Models;

namespace LiveLib.Application.Models.Authors
{
    public class AuthorDto : IMapWith<Author>
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string SecondName { get; set; } = string.Empty;

        public string ThirdName { get; set; } = string.Empty;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Author, AuthorDto>();
        }

    }
}
