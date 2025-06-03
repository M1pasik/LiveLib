using LiveLib.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LiveLib.PasswordHasher
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPasswordHasher(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IPassowrdHasher, PasswordHasher>();
            return services;
        }
    }
}