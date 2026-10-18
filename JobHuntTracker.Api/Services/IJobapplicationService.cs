using JobHuntTracker.Api.Models;

namespace JobHuntTracker.Api.Services;

public interface IJobApplicationService
{
    IEnumerable<JobApplication> GetAll(ApplicationStatus? status);
    JobApplication? GetById(Guid id);
    JobApplication Add(CreateJobApplicationDto dto);
    JobApplication? UpdateStatus(Guid id, ApplicationStatus status);
    bool Delete(Guid id);
}