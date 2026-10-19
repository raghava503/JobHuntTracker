using JobHuntTracker.Api.Models;

namespace JobHuntTracker.Api.Services;

public interface IJobApplicationService
{
    Task<List<JobApplication>> GetAllAsync(ApplicationStatus? status);
    Task<JobApplication?> GetByIdAsync(Guid id);
    Task<JobApplication> AddAsync(CreateJobApplicationDto dto);
    Task<JobApplication?> UpdateStatusAsync(Guid id, ApplicationStatus status);
    Task<bool> DeleteAsync(Guid id);
}