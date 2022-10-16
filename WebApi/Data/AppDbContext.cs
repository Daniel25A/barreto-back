using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

namespace WebApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opts):base(opts)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<Obligation> Obligations { get; set; } = null!;
    public DbSet<Movement> Movements { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;
}