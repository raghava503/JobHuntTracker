namespace JobHuntTracker.Api.Models;

public class User
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Email { get; set; } = "";
    public string PasswordHash { get; set; } = "";
}