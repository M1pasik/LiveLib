using LiveLib.Domain.Models;
using MediatR;

namespace LiveLib.Application.Features.Users.GetUsers
{
    public class GetUsersQuery : IRequest<ICollection<User>>
    {
    }
}
