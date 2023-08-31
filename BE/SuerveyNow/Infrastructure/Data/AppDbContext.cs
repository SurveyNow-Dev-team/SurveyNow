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
}