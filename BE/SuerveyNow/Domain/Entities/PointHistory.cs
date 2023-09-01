using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class PointHistory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Precision(2)]
    public DateTime Date { get; set; }
    
    [Column(TypeName = "nvarchar(500)")]
    public string? Description { get; set; }
    
    public PointHistoryType Type { get; set; }
    
    public int Point { get; set; }
    
    public TransactionStatus Status { get; set; }
    
    public long UserId { get; set; }

    public virtual User User { get; set; } = null!;
}