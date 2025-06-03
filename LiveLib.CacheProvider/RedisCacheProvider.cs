using System.Text.Json;
using LiveLib.Application.Interfaces;
using StackExchange.Redis;

namespace LiveLib.CacheProvider
{
    public class RedisCacheProvider : ICacheProvider
    {
        private readonly IDatabase _redis;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = false
        };

        public RedisCacheProvider(IConnectionMultiplexer connectionMultiplexer)
        {
            _redis = connectionMultiplexer?.GetDatabase() ??
                throw new ArgumentNullException(nameof(connectionMultiplexer));
        }

        public async Task<string?> StringGetAsync(string key, CancellationToken ct)
        {
            CheckKey(key);
            return await SafeExecuteAsync(() => _redis.StringGetAsync(key), ct);
        }

        public async Task StringSetAsync(string key, string value, CancellationToken ct, TimeSpan? expiry = null)
        {
            CheckKey(key);
            await SafeExecuteAsync(() => _redis.StringSetAsync(key, value, expiry), ct);
        }

        public async Task BytesSetAsync(string key, byte[] value, CancellationToken ct, TimeSpan? expiry = null)
        {
            CheckKey(key);
            await SafeExecuteAsync(() => _redis.StringSetAsync(key, value, expiry), ct);
        }

        public async Task<byte[]?> BytesGetAsync(string key, CancellationToken ct)
        {
            CheckKey(key);
            return (byte[]?)await SafeExecuteAsync(() => _redis.StringGetAsync(key), ct);
        }

        public async Task RemoveAsync(string key, CancellationToken ct)
        {
            CheckKey(key);
            await SafeExecuteAsync(() => _redis.KeyDeleteAsync(key), ct);
        }

        public async Task<string[]> SetGetAsync(string setKey, CancellationToken ct)
        {
            CheckKey(setKey);
            var data = await SafeExecuteAsync(() => _redis.SetMembersAsync(setKey), ct);
            return Array.ConvertAll(data, x => x.ToString());
        }

        public async Task SetAddAsync(string setKey, string value, CancellationToken ct, TimeSpan? expiry = null)
        {
            CheckKey(setKey);
            await SafeExecuteAsync(() => _redis.SetAddAsync(setKey, value), ct);
            if (expiry.HasValue)
            {
                await SafeExecuteAsync(() => _redis.KeyExpireAsync(setKey, expiry), ct);
            }
        }

        public async Task SetRemoveAsync(string setKey, string value, CancellationToken ct)
        {
            CheckKey(setKey);
            await SafeExecuteAsync(() => _redis.SetRemoveAsync(setKey, value), ct);
        }

        public async Task ObjectSetAsync<T>(string key, T value, CancellationToken ct, TimeSpan? expiry = null)
        {
            CheckKey(key);
            var json = JsonSerializer.Serialize(value, _jsonOptions);
            await SafeExecuteAsync(() => _redis.StringSetAsync(key, json, expiry), ct);
        }

        public async Task<T?> ObjectGetAsync<T>(string key, CancellationToken ct)
        {
            CheckKey(key);
            var value = await SafeExecuteAsync(() => _redis.StringGetAsync(key), ct);
            return value.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(value, _jsonOptions);
        }

        private static void CheckKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be null or empty", nameof(key));
        }

        private static async Task<T> SafeExecuteAsync<T>(Func<Task<T>> func, CancellationToken ct)
        {
            try
            {
                return await func().WaitAsync(ct);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Redis operation failed", ex);
            }
        }
    }
}