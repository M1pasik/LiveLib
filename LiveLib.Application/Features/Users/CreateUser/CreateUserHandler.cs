using AutoMapper;
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Interfaces;
using LiveLib.Domain.Models;
using MediatR;

namespace LiveLib.Application.Features.Users.CreateUser
{
    public class CreateUserHandler : HandlerBase, IRequestHandler<CreateUserCommand, Result<User>>
    {
        private readonly IPassowrdHasher _passwordHasher;

        public CreateUserHandler(IMapper mapper, IDatabaseContext context, IPassowrdHasher passwordHasher) : base(mapper, context)
        {
            _passwordHasher = passwordHasher;
        }

        public async Task<Result<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request);
            user.PasswordHash = _passwordHasher.Hash(request.Password);

            _context.Users.Add(user);
            var result = await _context.SaveChangesAsync(cancellationToken);

            return result == 0 ? Result<User>.Failure("User not created") : Result.Success(user);
        }
    }
}
