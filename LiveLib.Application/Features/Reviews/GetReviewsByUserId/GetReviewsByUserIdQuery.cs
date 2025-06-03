using LiveLib.Application.Models.Reviews;
using MediatR;

namespace LiveLib.Application.Features.Reviews.GetReviewsByUserId
{
    public record GetReviewsByUserIdQuery(Guid UserId) : IRequest<ICollection<ReviewDto>>;
}
