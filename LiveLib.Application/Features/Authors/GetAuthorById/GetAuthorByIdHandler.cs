using AutoMapper;
using AutoMapper.QueryableExtensions;
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Interfaces;
using LiveLib.Application.Models.Authors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiveLib.Application.Features.Authors.GetAuthorById
{
    public class GetAuthorByIdHandler : HandlerBase, IRequestHandler<GetAuthorByIdQuery, Result<AuthorDetailDto>>
    {
        public GetAuthorByIdHandler(IMapper mapper, IDatabaseContext context) : base(mapper, context)
        {
        }

        public async Task<Result<AuthorDetailDto>> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            var author = await _context.Authors
                .Where(a => a.Id == request.Id)
                .ProjectTo<AuthorDetailDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            return author is null ? Result<AuthorDetailDto>.NotFound($"Author {request.Id} not found") :
                Result.Success(author);
        }
    }
}
