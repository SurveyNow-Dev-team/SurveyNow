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

    [Column(TypeName = "nvarchar(50)")] 
    public string Email { get; set; } = null!;

    [Column(TypeName = "nvarchar(100)")] 
    public string GoogleId { get; set; } = null!;
    
    [RegularExpression(@"(84|0[3|5|7|8|9])+([0-9]{8})\b", ErrorMessage = "Invalid phone number.")]
    [Column(TypeName = "nvarchar(20)")]
    public string? PhoneNumber { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    public string FullName { get; set; } = null!;
    
    public Gender? Gender { get; set; }
    
    [Precision(2)]
    public DateTime? DateOfBirth { get; set; }
    
    [Column(TypeName = "nvarchar(100)")]
    public string? AvatarUrl { get; set; }
    
    [Precision(6, 1)]
    public decimal Point { get; set; } = 0;
    
    public UserStatus Status { get; set; } = UserStatus.Active;
    
    public Role Role { get; set; } = Role.User;
    
    public RelationshipStatus? RelationshipStatus { get; set; }
    
    public string? LangKey { get; set; } = "vn";
    
    public string? Currency { get; set; } = "VND";
    
    public bool IsDelete { get; set; } = false;
    
    [Precision(2)]
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    [Precision(2)] 
    public DateTime ModifiedDate { get; set; } = DateTime.Now;
    
    // public long? OccupationId { get; set; }
    //Shadow foreign key
    public virtual Occupation? Occupation { get; set; }
    
    // public long? AddressId { get; set; }
    //Shadow foreign key
    public virtual Address? Address { get; set; } // need to be optional to prevent cascade delete in many-to-many relationship
    
}