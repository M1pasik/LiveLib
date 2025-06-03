using AutoMapper;
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Interfaces;
using LiveLib.Domain.Models;
using MediatR;

namespace LiveLib.Application.Features.Authors.CreateAuthor
{
    public class CreateAuthorHandler : HandlerBase, IRequestHandler<CreateAuthorCommand, Result<Guid>>
    {
        public CreateAuthorHandler(IMapper mapper, IDatabaseContext context) : base(mapper, context)
        {
        }

        public async Task<Result<Guid>> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = _mapper.Map<Author>(request);
            _context.Authors.Add(author);
            var saved = await _context.SaveChangesAsync(cancellationToken);

            return saved == 0 ? Result<Guid>.ServerError("Author not saved") : Result.Success(author.Id);
        }
    }
}
