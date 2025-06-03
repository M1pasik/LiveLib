using AutoMapper;
using AutoMapper.QueryableExtensions;
using LiveLib.Application.Interfaces;
using LiveLib.Application.Models.Reviews;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiveLib.Application.Features.Reviews.GetReviewsByUserId
{
    public class GetReviewsByUserIdHandler : HandlerBase, IRequestHandler<GetReviewsByUserIdQuery, ICollection<ReviewDto>>
    {
        public GetReviewsByUserIdHandler(IMapper mapper, IDatabaseContext context) : base(mapper, context)
        {
        }

        public async Task<ICollection<ReviewDto>> Handle(GetReviewsByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Reviews
                .AsNoTracking()
                .Where(r => r.UserId == request.UserId)
                .ProjectTo<ReviewDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
