using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class UserReport
{
    public long Id { get; set; }

    [Column(TypeName = "nvarchar(100)")] public string? Type { get; set; }

    [Column(TypeName = "nvarchar(500)")] public string Reason { get; set; } = null!;

    public UserReportStatus Status { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "nvarchar(500)")] public string? Result { get; set; }

    public long CreatedUserId { get; set; }

    [ForeignKey(nameof(CreatedUserId))] public virtual User CreatedBy { get; set; } = null!;

    public long? UserId { get; set; }

    [ForeignKey(nameof(UserId))] public virtual User? User { get; set; }

    public long? SurveyId { get; set; }

    [ForeignKey(nameof(SurveyId))] public virtual Survey? Survey { get; set; }
}