using Microsoft.EntityFrameworkCore;
using Ticketing.Auth.Domain.Entities;

namespace Ticketing.Auth.Persistence;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
        base.OnModelCreating(modelBuilder);
    }
}