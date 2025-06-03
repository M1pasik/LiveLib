using LiveLib.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace LiveLib.CacheProvider
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCache(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration["Redis:Connection"];
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Redis connection string is missing");

            services.AddSingleton<IConnectionMultiplexer>(_ =>
                ConnectionMultiplexer.Connect(connectionString));

            services.AddScoped<ICacheProvider>(provider =>
                new RedisCacheProvider(provider.GetRequiredService<IConnectionMultiplexer>()));

            return services;
        }
    }
}