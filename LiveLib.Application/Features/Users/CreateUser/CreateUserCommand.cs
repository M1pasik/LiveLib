using LiveLib.Application.Common.Mapping;
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Domain.Models;
using MediatR;

namespace LiveLib.Application.Features.Users.CreateUser
{
    public class CreateUserCommand : IRequest<Result<User>>, IMapWith<User>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

    }
}
