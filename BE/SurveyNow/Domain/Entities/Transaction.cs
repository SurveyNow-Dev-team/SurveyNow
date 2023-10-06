using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using TransactionStatus = Domain.Enums.TransactionStatus;

namespace Domain.Entities;

public class Transaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public TransactionType TransactionType { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    [Precision(6,1)]
    [Range(0.1, 100000)]
    public decimal Point { get; set; }

    [Precision(9, 2)] public decimal Amount { get; set; }

    [Column(TypeName = "varchar(20)")] public string Currency { get; set; } = null!;

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime Date { get; set; } = DateTime.UtcNow;
    
    [Column(TypeName = "nvarchar(80)")] public string? SourceAccount { get; set; }

    [Column(TypeName = "nvarchar(80)")] public string? DestinationAccount { get; set; }

    [Column(TypeName = "nvarchar(80)")] public string? PurchaseCode { get; set; }

    public TransactionStatus Status { get; set; }

    public long UserId { get; set; }

    [ForeignKey(nameof(UserId))] public virtual User User { get; set; } = null!;
}