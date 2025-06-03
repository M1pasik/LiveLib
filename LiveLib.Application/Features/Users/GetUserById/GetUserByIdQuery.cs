using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Domain.Models;
using MediatR;

namespace LiveLib.Application.Features.Users.GetUserById
{
    public record GetUserByIdQuery(Guid userId) : IRequest<Result<User>>;
}
