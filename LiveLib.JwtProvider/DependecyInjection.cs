using LiveLib.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LiveLib.JwtProvider
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddJwtProvider(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IJwtProvider, JwtProvider>();
            return services;
        }
    }
}