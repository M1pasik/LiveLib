using AutoMapper;
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Interfaces;
using LiveLib.Application.Models.Reviews;
using MediatR;

namespace LiveLib.Application.Features.Reviews.UpdateReview
{
    public class UpdateReviewHandler : HandlerBase, IRequestHandler<UpdateReviewCommand, Result<ReviewDto>>
    {
        public UpdateReviewHandler(IMapper mapper, IDatabaseContext context) : base(mapper, context)
        {
        }

        public async Task<Result<ReviewDto>> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _context.Reviews.FindAsync(request.Id, cancellationToken);

            if (review == null)
            {
                return Result<ReviewDto>.NotFound($"Review {request.Id} not found");
            }

            _mapper.Map(request.Review, review);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(_mapper.Map<ReviewDto>(review));

        }
    }
}
