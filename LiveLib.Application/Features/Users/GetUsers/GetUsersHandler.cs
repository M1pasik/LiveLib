using AutoMapper;
using LiveLib.Application.Interfaces;
using LiveLib.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiveLib.Application.Features.Users.GetUsers
{
    public class GetUsersHandler : HandlerBase, IRequestHandler<GetUsersQuery, ICollection<User>>
    {
        public GetUsersHandler(IMapper mapper, IDatabaseContext context) : base(mapper, context)
        {
        }

        public async Task<ICollection<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return await _context.Users.AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
