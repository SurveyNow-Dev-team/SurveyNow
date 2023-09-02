using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
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
    public DbSet<Survey> Surveys { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionDetail> QuestionDetails { get; set; }
    public DbSet<UserSurvey> UserSurveys { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<UserReport> UserReports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<Question>()
        //     .HasKey(q => new { q.SurveyId, q.Order });
        
        modelBuilder.Entity<QuestionDetail>()
            .HasOne(qd => qd.Question)
            .WithMany(q => q.QuestionDetails)
            .HasForeignKey(qd => new { qd.SurveyId, qd.QuestionOrder });
    }
}