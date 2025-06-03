using LiveLib.Application.Commom.ResultWrapper;
using MediatR;

namespace LiveLib.Application.Features.Users.DeleteUser
{
    public record DeleteUserCommand(Guid UserId) : IRequest<Result>;
}
