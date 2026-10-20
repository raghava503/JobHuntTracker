using System.ComponentModel.DataAnnotations;

namespace JobHuntTracker.Api.Models;

public record RegisterDto
{
    [Required, EmailAddress, StringLength(200)]
    public string Email { get; init; } = "";

    [Required, StringLength(100, MinimumLength = 8)]
    public string Password { get; init; } = "";
}

public record LoginDto
{
    [Required] public string Email { get; init; } = "";
    [Required] public string Password { get; init; } = "";
}

public record AuthResponse(string Token, DateTime ExpiresAtUtc);