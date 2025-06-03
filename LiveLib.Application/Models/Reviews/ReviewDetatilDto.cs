using LiveLib.Application.Common.Mapping;
using LiveLib.Application.Models.Books;
using LiveLib.Application.Models.Users;
using LiveLib.Domain.Models;

namespace LiveLib.Application.Models.Reviews
{
    public class ReviewDetatilDto : IMapWith<Review>
    {
        public Guid UserId { get; set; }
        public UserDto User { get; set; } = null!;
        public BookDto Book { get; set; } = null!;
        public double Rate { get; set; }
        public string Comment { get; set; } = string.Empty;
        public bool IsRecommended { get; set; }

    }
}
