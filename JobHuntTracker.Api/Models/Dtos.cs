using System.ComponentModel.DataAnnotations;

namespace JobHuntTracker.Api.Models;

public record CreateJobApplicationDto
{
    [Required, StringLength(100)]
    public string Company { get; init; } = "";

    [Required, StringLength(100)]
    public string Role { get; init; } = "";
}

public record UpdateStatusDto
{
    [EnumDataType(typeof(ApplicationStatus))]
    public ApplicationStatus Status { get; init; }
}