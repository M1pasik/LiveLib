using AutoMapper;
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Interfaces;
using LiveLib.Application.Models.Authors;
using MediatR;

namespace LiveLib.Application.Features.Authors.UpdateAuthor
{
    public class UpdateAuthorHandler : HandlerBase, IRequestHandler<UpdateAuthorCommand, Result<AuthorDetailDto>>
    {
        public UpdateAuthorHandler(IMapper mapper, IDatabaseContext context) : base(mapper, context)
        {
        }

        public async Task<Result<AuthorDetailDto>> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = await _context.Authors.FindAsync(request.Id, cancellationToken);

            if (author == null)
            {
                return Result<AuthorDetailDto>.NotFound($"Author {request.Id} not found");
            }

            _mapper.Map(request.Author, author);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(_mapper.Map<AuthorDetailDto>(author));
        }
    }
}
