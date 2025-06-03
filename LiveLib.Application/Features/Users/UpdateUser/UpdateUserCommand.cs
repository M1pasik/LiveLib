using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Domain.Models;
using MediatR;

namespace LiveLib.Application.Features.Users.UpdateUser
{
    public record UpdateUserCommand(Guid Id, UserUpdateDto user) : IRequest<Result<User>>;
}
