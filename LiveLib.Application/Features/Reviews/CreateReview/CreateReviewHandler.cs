using AutoMapper;
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Interfaces;
using LiveLib.Domain.Models;
using MediatR;

namespace LiveLib.Application.Features.Reviews.CreateReview
{
    public class CreateReviewHandler : HandlerBase, IRequestHandler<CreateReviewCommand, Result<Guid>>
    {
        public CreateReviewHandler(IMapper mapper, IDatabaseContext context) : base(mapper, context)
        {
        }

        public async Task<Result<Guid>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = _mapper.Map<Review>(request);
            _context.Reviews.Add(review);
            var saved = await _context.SaveChangesAsync(cancellationToken);

            return saved == 0 ? Result<Guid>.ServerError("Review not saved") : Result.Success(review.Id);
        }
    }
}
