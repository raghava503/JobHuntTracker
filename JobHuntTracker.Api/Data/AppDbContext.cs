using JobHuntTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace JobHuntTracker.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<JobApplication> JobApplications => Set<JobApplication>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.Property(u => u.Email).IsRequired().HasMaxLength(200);
            e.HasIndex(u => u.Email).IsUnique();
        });

        modelBuilder.Entity<JobApplication>(e =>
        {
            e.Property(a => a.Company).IsRequired().HasMaxLength(100);
            e.Property(a => a.Role).IsRequired().HasMaxLength(100);
            e.Property(a => a.Status).HasConversion<string>();
            e.HasIndex(a => a.Status);
            e.HasIndex(a => a.UserId);
            e.HasOne<User>().WithMany().HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Cascade);
        });
    }
}