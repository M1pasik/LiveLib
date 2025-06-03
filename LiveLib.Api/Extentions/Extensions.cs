using System;
using System.Security.Claims;

namespace LiveLib.Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid Id(this ClaimsPrincipal user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var claimValue = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(claimValue))
                throw new InvalidOperationException("User ID claim (NameIdentifier) not found");

            if (!Guid.TryParse(claimValue, out var userId))
                throw new InvalidOperationException($"User ID '{claimValue}' is not a valid Guid");

            return userId;
        }

        public static Guid? TryGetId(this ClaimsPrincipal user)
        {
            try
            {
                return user.Id();
            }
            catch
            {
                return null;
            }
        }
    }
}