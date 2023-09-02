using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TransactionStatus = Domain.Enums.TransactionStatus;

namespace Domain.Entities;

public class PointPurchase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Precision(9, 2)] public decimal Amount { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime Date { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "nvarchar(20)")] public string Type { get; set; } = null!;

    [Column(TypeName = "nvarchar(80)")] public string Account { get; set; } = null!;

    [Column(TypeName = "nvarchar(80)")] public string DestinationAccount { get; set; } = null!;

    [Column(TypeName = "nvarchar(80)")] public string PurchaseCode { get; set; } = null!;

    public int Point { get; set; }

    [Column(TypeName = "varchar(20)")] public string Currency { get; set; } = null!;

    public TransactionStatus Status { get; set; }

    public long UserId { get; set; }

    public virtual User User { get; set; } = null!;
}