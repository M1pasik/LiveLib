using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Models.Reviews;
using MediatR;

namespace LiveLib.Application.Features.Reviews.UpdateReview
{
    public record UpdateReviewCommand(Guid Id, UpdateReviewDto Review) : IRequest<Result<ReviewDto>>;
}
