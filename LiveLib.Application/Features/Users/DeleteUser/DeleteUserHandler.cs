using AutoMapper;
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Interfaces;
using MediatR;

namespace LiveLib.Application.Features.Users.DeleteUser
{
    public class DeleteUserHandler : HandlerBase, IRequestHandler<DeleteUserCommand, Result>
    {
        public DeleteUserHandler(IMapper mapper, IDatabaseContext context) : base(mapper, context)
        {
        }

        public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(request.UserId, cancellationToken);

            if (user == null)
            {
                return Result.Failure("User not found");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
