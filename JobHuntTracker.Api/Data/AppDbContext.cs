using JobHuntTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace JobHuntTracker.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<JobApplication> JobApplications => Set<JobApplication>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JobApplication>(e =>
        {
            e.Property(a => a.Company).IsRequired().HasMaxLength(100);
            e.Property(a => a.Role).IsRequired().HasMaxLength(100);
            e.Property(a => a.Status).HasConversion<string>();   // store "Interview", not 1
            e.HasIndex(a => a.Status);
        });
    }
}