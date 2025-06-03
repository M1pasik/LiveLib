using LiveLib.Domain.Models;

namespace LiveLib.Application.Interfaces
{
    public interface ITokenService
    {
        Task AddRefreshTokenAsync(RefreshToken token, CancellationToken ct);
        Task<RefreshToken?> GetActiveTokenAsync(string userRefreshToken, CancellationToken ct);
        Task<RefreshToken?> GetActiveTokenByIdAsync(Guid tokenId, CancellationToken ct);
        IAsyncEnumerable<RefreshToken> GetActiveTokensByUserIdAsync(Guid userId, CancellationToken ct);
        Task RevokeTokenAsync(RefreshToken token, CancellationToken ct);
    }
}