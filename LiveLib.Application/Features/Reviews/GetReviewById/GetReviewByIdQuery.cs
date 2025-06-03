using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Models.Reviews;
using MediatR;

namespace LiveLib.Application.Features.Reviews.GetReviewById
{
    public record GetReviewByIdQuery(Guid Id) : IRequest<Result<ReviewDetatilDto>>;
}
