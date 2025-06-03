using LiveLib.Application.Common.Mapping;
using LiveLib.Domain.Models;

namespace LiveLib.Application.Features.Reviews.UpdateReview
{
    public class UpdateReviewDto : IMapWith<Review>
    {
        public double? Rate { get; set; }
        public string? Comment { get; set; }
        public bool? IsRecommended { get; set; }
    }
}
