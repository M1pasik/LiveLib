
namespace LiveLib.Application.Interfaces
{
    public interface ICacheProvider
    {
        Task<string?> GetStringAsync(string key, CancellationToken ct = default);
        Task SetStringAsync(string key, string value, CancellationToken ct = default, TimeSpan? expiry = null);

        Task<byte[]?> GetBytesAsync(string key, CancellationToken ct = default);
        Task SetBytesAsync(string key, byte[] value, CancellationToken ct = default, TimeSpan? expiry = null);

        Task<T?> GetObjectAsync<T>(string key, CancellationToken ct = default);
        Task SetObjectAsync<T>(string key, T value, CancellationToken ct = default, TimeSpan? expiry = null);

        Task AddToSetAsync(string setKey, string value, CancellationToken ct = default, TimeSpan? expiry = null);
        Task<string[]> GetSetAsync(string setKey, CancellationToken ct = default);
        Task RemoveFromSetAsync(string setKey, string value, CancellationToken ct = default);

        Task RemoveAsync(string key, CancellationToken ct = default);
        object SetAddAsync(string v, string token, CancellationToken ct, TimeSpan expiration);
        object StringSetAsync(string v1, string v2, CancellationToken ct, TimeSpan expiration);
        ReadOnlySpan<Task> ObjectSetAsync(string v, string tokenJson, CancellationToken ct, TimeSpan expiration);
        object SetRemoveAsync(string v, string token, CancellationToken ct);
        Task BytesSetAsync(string v, byte[] imageBytes, CancellationToken cancellationToken);
        Task<string?> StringGetAsync(string v, CancellationToken ct);
        Task<IEnumerable<object>> SetGetAsync(string v, CancellationToken ct);
        Task<byte[]> BytesGetAsync(string v, CancellationToken cancellationToken);
    }
}