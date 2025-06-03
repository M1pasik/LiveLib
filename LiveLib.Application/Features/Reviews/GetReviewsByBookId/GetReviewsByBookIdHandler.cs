using AutoMapper;
using AutoMapper.QueryableExtensions;
using LiveLib.Application.Interfaces;
using LiveLib.Application.Models.Reviews;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiveLib.Application.Features.Reviews.GetReviewsByBookId
{
    public class GetReviewsByBookIdHandler : HandlerBase, IRequestHandler<GetReviewsByBookIdQuery, ICollection<ReviewDto>>
    {
        public GetReviewsByBookIdHandler(IMapper mapper, IDatabaseContext context) : base(mapper, context)
        {
        }

        public async Task<ICollection<ReviewDto>> Handle(GetReviewsByBookIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Reviews
                .AsNoTracking()
                .Where(c => c.BookId == request.Id)
                .ProjectTo<ReviewDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
