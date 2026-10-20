namespace JobHuntTracker.Web;

public enum ApplicationStatus { Applied, Interview, Offer, Rejected }

public record JobApplication(Guid Id, string Company, string Role,
                             ApplicationStatus Status, DateTime AppliedOnUtc);

public record AuthResponse(string Token, DateTime ExpiresAtUtc);
public record Credentials(string Email, string Password);
public record NewApplication(string Company, string Role);
public record StatusChange(ApplicationStatus Status);