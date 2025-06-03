using AutoMapper;
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Interfaces;
using LiveLib.Domain.Models;
using MediatR;

namespace LiveLib.Application.Features.Users.UpdateUser
{
    public class UpdateUserHandler : HandlerBase, IRequestHandler<UpdateUserCommand, Result<User>>
    {
        private readonly IPassowrdHasher _passowrdHasher;

        public UpdateUserHandler(IMapper mapper, IDatabaseContext context, IPassowrdHasher passowrdHasher) : base(mapper, context)
        {
            _passowrdHasher = passowrdHasher;
        }

        public async Task<Result<User>> Handle(UpdateUserCommand reqwest, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(reqwest.Id, cancellationToken);

            if (user == null)
            {
                return Result<User>.Failure("User not found");
            }

            _mapper.Map(reqwest, user);
            if (reqwest.user.Password != null)
            {
                user.PasswordHash = _passowrdHasher.Hash(reqwest.user.Password);
            }

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success(user);
        }
    }
}
