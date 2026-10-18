using System.Collections.Concurrent;
using JobHuntTracker.Api.Models;

namespace JobHuntTracker.Api.Services;

public class InMemoryJobApplicationService : IJobApplicationService
{
    private readonly ConcurrentDictionary<Guid, JobApplication> _store = new();

    public IEnumerable<JobApplication> GetAll(ApplicationStatus? status) =>
        _store.Values
              .Where(a => status is null || a.Status == status)
              .OrderByDescending(a => a.AppliedOnUtc);

    public JobApplication? GetById(Guid id) =>
        _store.TryGetValue(id, out var app) ? app : null;

    public JobApplication Add(CreateJobApplicationDto dto)
    {
        var app = new JobApplication { Company = dto.Company, Role = dto.Role };
        _store[app.Id] = app;
        return app;
    }

    public JobApplication? UpdateStatus(Guid id, ApplicationStatus status)
    {
        if (!_store.TryGetValue(id, out var app)) return null;
        app.Status = status;
        return app;
    }

    public bool Delete(Guid id) => _store.TryRemove(id, out _);
}