using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace JobHuntTracker.Api.Services;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user) =>
        Guid.Parse(user.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
}