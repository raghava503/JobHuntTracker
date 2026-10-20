using JobHuntTracker.Api.Data;
using JobHuntTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace JobHuntTracker.Api.Services;

public class EfJobApplicationService(AppDbContext db) : IJobApplicationService
{
    public async Task<List<JobApplication>> GetAllAsync(Guid userId, ApplicationStatus? status)
    {
        var query = db.JobApplications.AsNoTracking().Where(a => a.UserId == userId);

        if (status is not null)
            query = query.Where(a => a.Status == status);

        return await query.OrderByDescending(a => a.AppliedOnUtc).ToListAsync();
    }

    public async Task<JobApplication?> GetByIdAsync(Guid userId, Guid id) =>
        await db.JobApplications.AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

    public async Task<JobApplication> AddAsync(Guid userId, CreateJobApplicationDto dto)
    {
        var app = new JobApplication { Company = dto.Company, Role = dto.Role, UserId = userId };
        db.JobApplications.Add(app);
        await db.SaveChangesAsync();
        return app;
    }

    public async Task<JobApplication?> UpdateStatusAsync(Guid userId, Guid id, ApplicationStatus status)
    {
        var app = await db.JobApplications.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);
        if (app is null) return null;

        app.Status = status;
        await db.SaveChangesAsync();
        return app;
    }

    public async Task<bool> DeleteAsync(Guid userId, Guid id) =>
        await db.JobApplications.Where(a => a.Id == id && a.UserId == userId)
                                .ExecuteDeleteAsync() > 0;
}