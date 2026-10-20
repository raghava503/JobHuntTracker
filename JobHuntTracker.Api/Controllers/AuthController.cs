using JobHuntTracker.Api.Data;
using JobHuntTracker.Api.Models;
using JobHuntTracker.Api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobHuntTracker.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(AppDbContext db, IPasswordHasher<User> hasher, ITokenService tokens) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterDto dto)
    {
        string email = dto.Email.Trim().ToLowerInvariant();

        if (await db.Users.AnyAsync(u => u.Email == email))
            return Conflict(new { error = "That email is already registered." });

        var user = new User { Email = email };
        user.PasswordHash = hasher.HashPassword(user, dto.Password);

        db.Users.Add(user);
        await db.SaveChangesAsync();

        return StatusCode(StatusCodes.Status201Created, tokens.CreateToken(user));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginDto dto)
    {
        string email = dto.Email.Trim().ToLowerInvariant();
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email);

        bool valid = user is not null &&
            hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password)
                != PasswordVerificationResult.Failed;

        // Same message for "no such user" and "wrong password"
        return valid ? Ok(tokens.CreateToken(user!))
                     : Unauthorized(new { error = "Invalid email or password." });
    }
}