namespace JobHuntTracker.Api.Models;

public enum ApplicationStatus { Applied, Interview, Offer, Rejected }

public class JobApplication
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Company { get; set; } = "";
    public string Role { get; set; } = "";
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;
    public DateTime AppliedOnUtc { get; init; } = DateTime.UtcNow;
}