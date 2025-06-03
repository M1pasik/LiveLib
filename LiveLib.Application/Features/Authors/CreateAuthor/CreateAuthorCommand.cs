
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Common.Mapping;
using LiveLib.Domain.Models;
using MediatR;

namespace LiveLib.Application.Features.Authors.CreateAuthor
{
    public class CreateAuthorCommand : IRequest<Result<Guid>>, IMapWith<Author>
    {
        public string FirstName { get; set; } = string.Empty;

        public string SecondName { get; set; } = string.Empty;

        public string ThirdName { get; set; } = string.Empty;

        public string Biography { get; set; } = string.Empty;
    }
}
