using AutoMapper;
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Interfaces;
using LiveLib.Application.Models.Authors;
using MediatR;

namespace LiveLib.Application.Features.Authors.DeleteAuthor
{
    public class DeleteAuthorHandler : HandlerBase, IRequestHandler<DeleteAuthorCommand, Result<AuthorDetailDto>>
    {
        public DeleteAuthorHandler(IMapper mapper, IDatabaseContext context) : base(mapper, context)
        {
        }

        public async Task<Result<AuthorDetailDto>> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = await _context.Authors.FindAsync(request.Id, cancellationToken);
            if (author == null)
            {
                return Result<AuthorDetailDto>.NotFound($"Author {request.Id} not found");
            }

            _context.Authors.Remove(author);
            var removed = await _context.SaveChangesAsync(cancellationToken);

            return removed == 0 ? Result<AuthorDetailDto>.NotFound($"Author {request.Id} not deleted")
                : Result.Success(_mapper.Map<AuthorDetailDto>(author));

        }
    }
}
