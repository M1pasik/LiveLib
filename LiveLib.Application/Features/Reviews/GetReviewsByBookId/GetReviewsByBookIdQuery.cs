using LiveLib.Application.Models.Reviews;
using MediatR;

namespace LiveLib.Application.Features.Reviews.GetReviewsByBookId
{
    public record GetReviewsByBookIdQuery(Guid Id) : IRequest<ICollection<ReviewDto>>;
}
