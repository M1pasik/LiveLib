using System.Text.Json.Serialization;

namespace LiveLib.Domain.Models
{
    public class RefreshToken
    {
        public Guid Id { get; set; }

        public string Token { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public DateTime? RevokedAt { get; set; }

        [JsonIgnore]
        public bool IsActive { get => RevokedAt == null; }

        public Guid UserId { get; set; }
    }
}
