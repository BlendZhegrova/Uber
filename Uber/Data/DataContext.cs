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

}