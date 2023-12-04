using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Uber.Domain;

namespace Uber.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }
    public DbSet<RefreshToken>? RefreshTokens { get; set; }
    public DbSet<Driver>? Drivers { get; set; }
    public DbSet<Customer>? Customers { get; set; }
    public DbSet<Trip> Trips { get; set; }
    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Trip>()
            .HasOne(t => t.Driver)
            .WithOne()
            .HasForeignKey<Driver>(t => t.Id);
        
        modelBuilder.Entity<Trip>()
            .HasOne(t => t.Customer)
            .WithOne()
            .HasForeignKey<Customer>(t => t.Id);

        modelBuilder.Entity<Payment>()
            .HasOne(t => t.Trip)
            .WithOne()
            .HasForeignKey<Trip>(t => t.Id);
    }
}