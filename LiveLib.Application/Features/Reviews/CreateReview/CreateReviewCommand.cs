using System.Text.Json.Serialization;
using LiveLib.Application.Common.Mapping;
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Domain.Models;
using MediatR;

namespace LiveLib.Application.Features.Reviews.CreateReview
{
    public class CreateReviewCommand : IRequest<Result<Guid>>, IMapWith<Review>
    {
        [JsonIgnore]
        public Guid? UserId { get; set; }
        public Guid BookId { get; set; }
        public double Rate { get; set; }
        public string Comment { get; set; } = string.Empty;
        public bool IsRecommended { get; set; }

    }
}
