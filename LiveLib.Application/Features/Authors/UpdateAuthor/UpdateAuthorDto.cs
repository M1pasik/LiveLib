using AutoMapper;
using LiveLib.Application.Common.Mapping;
using LiveLib.Domain.Models;

namespace LiveLib.Application.Features.Authors.UpdateAuthor
{
    public class UpdateAuthorDto : IMapWith<Author>
    {
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? ThirdName { get; set; }
        public string? Biography { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateAuthorDto, Author>()
                .ForAllMembers(opts => opts.Condition((src, dest, field) => field != null));
        }
    }
}
