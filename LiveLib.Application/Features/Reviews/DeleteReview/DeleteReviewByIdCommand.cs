using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Models.Reviews;
using MediatR;

namespace LiveLib.Application.Features.Reviews.DeleteReview
{
    public record DeleteReviewByIdCommand(Guid Id) : IRequest<Result<ReviewDto>>;
}
