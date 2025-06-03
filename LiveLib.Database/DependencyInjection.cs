using System.Runtime.CompilerServices;
using System.Text.Json;
using LiveLib.Application.Interfaces;
using LiveLib.Domain.Models;

namespace LiveLib.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly ICacheProvider _cache;

        public TokenService(ICacheProvider cache)
        {
            _cache = cache;
        }

        public async Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken ct)
        {
            if (refreshToken == null) return;

            try
            {
                var expiration = refreshToken.ExpiresAt - DateTime.UtcNow;
                var tokenJson = JsonSerializer.Serialize(refreshToken);

                await Task.WhenAll(
                    _cache.ObjectSetAsync($"token:{refreshToken.Id}", tokenJson, ct, expiration),
                    _cache.StringSetAsync($"tokenId:{refreshToken.Token}", refreshToken.Id.ToString(), ct, expiration),
                    _cache.SetAddAsync($"user:{refreshToken.UserId}:tokens", refreshToken.Token, ct, expiration)
                );
            }
            catch
            {
                throw;
            }
        }

        public async Task<RefreshToken?> GetActiveTokenAsync(string userRefreshToken, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(userRefreshToken)) return null;

            try
            {
                var tokenId = await _cache.StringGetAsync($"tokenId:{userRefreshToken}", ct);
                if (string.IsNullOrEmpty(tokenId)) return null;

                var tokenString = await _cache.StringGetAsync($"token:{tokenId}", ct);
                return string.IsNullOrEmpty(tokenString)
                    ? null
                    : JsonSerializer.Deserialize<RefreshToken>(tokenString);
            }
            catch
            {
                return null;
            }
        }

        public async Task<RefreshToken?> GetActiveTokenByIdAsync(Guid tokenId, CancellationToken ct)
        {
            if (tokenId == Guid.Empty) return null;

            try
            {
                var tokenString = await _cache.StringGetAsync($"token:{tokenId}", ct);
                return string.IsNullOrEmpty(tokenString)
                    ? null
                    : JsonSerializer.Deserialize<RefreshToken>(tokenString);
            }
            catch
            {
                return null;
            }
        }

        public async Task RevokeTokenAsync(RefreshToken refreshToken, CancellationToken ct)
        {
            if (refreshToken == null) return;

            try
            {
                await Task.WhenAll(
                    _cache.RemoveAsync($"token:{refreshToken.Id}", ct),
                    _cache.RemoveAsync($"tokenId:{refreshToken.Token}", ct),
                    _cache.SetRemoveAsync($"user:{refreshToken.UserId}:tokens", refreshToken.Token, ct)
                );
            }
            catch
            {
                throw;
            }
        }

        public async IAsyncEnumerable<RefreshToken> GetActiveTokensByUserIdAsync(
            Guid userId,
            [EnumeratorCancellation] CancellationToken ct)
        {
            if (userId == Guid.Empty) yield break;

            IEnumerable<string> tokens = Array.Empty<string>();

            try
            {
                tokens = await _cache.SetGetAsync($"user:{userId}:tokens", ct);
            }
            catch
            {
                yield break;
            }

            foreach (var token in tokens)
            {
                ct.ThrowIfCancellationRequested();

                RefreshToken? refreshToken = null;
                try
                {
                    var tokenId = await _cache.StringGetAsync($"tokenId:{token}", ct);
                    if (!string.IsNullOrEmpty(tokenId))
                    {
                        var tokenString = await _cache.StringGetAsync($"token:{tokenId}", ct);
                        refreshToken = string.IsNullOrEmpty(tokenString)
                            ? null
                            : JsonSerializer.Deserialize<RefreshToken>(tokenString);
                    }
                }
                catch
                {

                }

                if (refreshToken != null)
                {
                    yield return refreshToken;
                }
            }
        }
    }
}