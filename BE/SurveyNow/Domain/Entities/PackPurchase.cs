using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class PackPurchase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    
    [Precision(2)]
    public DateTime Date { get; set; }
    
    public PackType PackType { get; set; }
    
    public bool IsUsePoint { get; set; }
    
    public int? Point { get; set; }
    
    public int? Quantity { get; set; }
    
    [Column(TypeName = "nvarchar(20)")]
    public string? Type { get; set; }
    
    [Column(TypeName = "nvarchar(80)")]
    public string? Account { get; set; }
    
    [Precision(9,2)]
    public decimal? Amount { get; set; }
    
    [Column(TypeName = "nvarchar(80)")]
    public string? DestinationAccount { get; set; }
    
    [Column(TypeName = "nvarchar(80)")]
    public string? PurchaseCode { get; set; }
    
    [Column(TypeName = "varchar(20)")]
    public string? Currency { get; set; }

    public TransactionStatus Status { get; set; }

    public long UserId { get; set; }
    
    public virtual User User { get; set; } = null!;
}