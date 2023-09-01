using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Occupation> Occupations { get; set; }
    public DbSet<Field> Fields { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Province> Provinces { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<Hobby> Hobbies { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<PointPurchase> PointPurchases { get; set; }
    public DbSet<PackPurchase> PackPurchases { get; set; }
    public DbSet<PointHistory> PointHistories { get; set; }
}