using AutoMapper;
using AutoMapper.QueryableExtensions;
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Interfaces;
using LiveLib.Application.Models.Reviews;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiveLib.Application.Features.Reviews.GetReviewById
{
    public class GetReviewByIdHandler : HandlerBase, IRequestHandler<GetReviewByIdQuery, Result<ReviewDetatilDto>>
    {
        public GetReviewByIdHandler(IMapper mapper, IDatabaseContext context) : base(mapper, context)
        {
        }

        public async Task<Result<ReviewDetatilDto>> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
        {
            var genre = await _context.Reviews
                .Where(r => r.Id == request.Id)
                .ProjectTo<ReviewDetatilDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            return genre is null ? Result<ReviewDetatilDto>.NotFound($"Review {request.Id} not found") :
                Result.Success(genre);
        }
    }
}
