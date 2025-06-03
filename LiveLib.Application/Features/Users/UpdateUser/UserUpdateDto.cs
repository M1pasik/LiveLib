using LiveLib.Application.Common.Mapping;
using LiveLib.Domain.Models;

namespace LiveLib.Application.Features.Users.UpdateUser
{
    public class UserUpdateDto : IMapWith<User>
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
    }
}
