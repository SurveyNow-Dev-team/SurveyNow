using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Occupation> Occupations { get; set; }
    public virtual DbSet<Field> Fields { get; set; }
    public virtual DbSet<Position> Positions { get; set; }
    public virtual DbSet<Address> Addresses { get; set; }
    public virtual DbSet<Province> Provinces { get; set; }
    public virtual DbSet<City> Cities { get; set; }
    public virtual DbSet<District> Districts { get; set; }
    public virtual DbSet<Hobby> Hobbies { get; set; }
    public virtual DbSet<Payment> Payments { get; set; }
    public virtual DbSet<Transaction> Transactions { get; set; }
    public virtual DbSet<PackPurchase> PackPurchases { get; set; }
    public virtual DbSet<PointHistory> PointHistories { get; set; }
    public virtual DbSet<Survey> Surveys { get; set; }
    public virtual DbSet<Section> Sections { get; set; }
    public virtual DbSet<Question> Questions { get; set; }
    public virtual DbSet<RowOption> RowOptions { get; set; }
    public virtual DbSet<ColumnOption> ColumnOptions { get; set; }
    public virtual DbSet<UserSurvey> UserSurveys { get; set; }
    public virtual DbSet<Answer> Answers { get; set; }
    public virtual DbSet<AnswerOption> AnswerOptions { get; set; }
    public virtual DbSet<UserReport> UserReports { get; set; }
    public virtual DbSet<Criterion> Criteria { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<Question>()
        //     .HasKey(q => new { q.SurveyId, q.Order });

        // modelBuilder.Entity<QuestionDetail>()
        //     .HasOne(qd => qd.Question)
        //     .WithMany(q => q.QuestionDetails)
        //     .HasForeignKey(qd => new { qd.SurveyId, qd.QuestionOrder });

        //Read only property constraints
        modelBuilder.Entity<User>()
            .Property(u => u.CreatedDate)
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

        modelBuilder.Entity<UserSurvey>()
            .Property(u => u.Date)
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

        modelBuilder.Entity<UserReport>()
            .Property(u => u.CreatedDate)
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

        modelBuilder.Entity<UserReport>()
            .Property(u => u.CreatedUserId)
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

        modelBuilder.Entity<UserReport>()
            .Property(u => u.UserId)
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

        modelBuilder.Entity<UserReport>()
            .Property(u => u.SurveyId)
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

        modelBuilder.Entity<Survey>()
            .Property(u => u.CreatedDate)
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

        modelBuilder.Entity<Survey>()
            .Property(u => u.CreatedUserId)
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

        modelBuilder.Entity<Transaction>()
            .Property(u => u.Date)
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

        modelBuilder.Entity<PointHistory>()
            .Property(u => u.Date)
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

        modelBuilder.Entity<PackPurchase>()
            .Property(u => u.Date)
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        //Read only property constraints
    }
}