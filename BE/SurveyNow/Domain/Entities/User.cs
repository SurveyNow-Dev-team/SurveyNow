using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(Email), IsUnique = true)]
[Index(nameof(GoogleId), IsUnique = true)]
[Index(nameof(PhoneNumber), IsUnique = true)]
public class User
{
    [Key]
    [DatabaseGenerated((DatabaseGeneratedOption.Identity))]
    public long Id { get; set; }

    [Column(TypeName = "nvarchar(50)")] public string Email { get; set; } = null!;

    [Column(TypeName = "nvarchar(100)")] public string? GoogleId { get; set; }

    [Column(TypeName = "varchar(100)")] public string? PasswordHash { get; set; }

    [RegularExpression(@"(84|0[3|5|7|8|9])+([0-9]{8})\b", ErrorMessage = "Invalid phone number.")]
    [Column(TypeName = "nvarchar(20)")]
    public string? PhoneNumber { get; set; }

    [Column(TypeName = "nvarchar(50)")] public string FullName { get; set; } = null!;

    public Gender? Gender { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime? DateOfBirth { get; set; }

    [Column(TypeName = "nvarchar(150)")] public string? AvatarUrl { get; set; }

    [Precision(6, 1)] [Range(0, 100000)] public decimal Point { get; set; } = 0;

    public UserStatus Status { get; set; } = UserStatus.Active;

    public Role Role { get; set; } = Role.User;

    public RelationshipStatus? RelationshipStatus { get; set; }

    public string? LangKey { get; set; } = "vn";

    public string? Currency { get; set; } = "VND";

    public bool IsDelete { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

    // public long? OccupationId { get; set; }
    //Shadow foreign key
    public virtual Occupation? Occupation { get; set; }

    // public long? AddressId { get; set; }
    //Shadow foreign key
    public virtual Address? Address { get; set; }

    public virtual ICollection<UserSurvey> UserSurveys { get; set; } = new List<UserSurvey>();

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual ICollection<Survey> Surveys { get; set; } = new List<Survey>();

    public virtual ICollection<Hobby> Hobbies { get; set; } = new List<Hobby>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Transaction> PointPurchases { get; set; } = new List<Transaction>();

    public virtual ICollection<PackPurchase> PackPurchases { get; set; } = new List<PackPurchase>();

    public virtual ICollection<PointHistory> PointHistories { get; set; } = new List<PointHistory>();
}