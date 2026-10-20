using JobHuntTracker.Api.Models;

namespace JobHuntTracker.Api.Services;

public interface IJobApplicationService
{
    Task<List<JobApplication>> GetAllAsync(Guid userId, ApplicationStatus? status);
    Task<JobApplication?> GetByIdAsync(Guid userId, Guid id);
    Task<JobApplication> AddAsync(Guid userId, CreateJobApplicationDto dto);
    Task<JobApplication?> UpdateStatusAsync(Guid userId, Guid id, ApplicationStatus status);
    Task<bool> DeleteAsync(Guid userId, Guid id);
}