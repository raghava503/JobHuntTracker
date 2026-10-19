using JobHuntTracker.Api.Data;
using JobHuntTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace JobHuntTracker.Api.Services;

public class EfJobApplicationService(AppDbContext db) : IJobApplicationService
{
    public async Task<List<JobApplication>> GetAllAsync(ApplicationStatus? status)
    {
        var query = db.JobApplications.AsNoTracking();

        if (status is not null)
            query = query.Where(a => a.Status == status);

        return await query.OrderByDescending(a => a.AppliedOnUtc).ToListAsync();
    }

    public async Task<JobApplication?> GetByIdAsync(Guid id) =>
        await db.JobApplications.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);

    public async Task<JobApplication> AddAsync(CreateJobApplicationDto dto)
    {
        var app = new JobApplication { Company = dto.Company, Role = dto.Role };
        db.JobApplications.Add(app);
        await db.SaveChangesAsync();
        return app;
    }

    public async Task<JobApplication?> UpdateStatusAsync(Guid id, ApplicationStatus status)
    {
        var app = await db.JobApplications.FindAsync(id);
        if (app is null) return null;

        app.Status = status;
        await db.SaveChangesAsync();
        return app;
    }

    public async Task<bool> DeleteAsync(Guid id) =>
        await db.JobApplications.Where(a => a.Id == id).ExecuteDeleteAsync() > 0;
}