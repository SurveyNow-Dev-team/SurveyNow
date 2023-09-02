using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(Type), nameof(UserId), IsUnique = true)]
public class Payment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    
    [Column(TypeName = "nvarchar(20)")]
    public string Type { get; set; } = null!;
    
    [Column(TypeName = "nvarchar(80)")]
    public string Account { get; set; } = null!;

    public bool IsDefault { get; set; }

    public long UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;
}