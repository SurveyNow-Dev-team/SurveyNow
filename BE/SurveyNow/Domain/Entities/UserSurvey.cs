using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(SurveyId), nameof(UserId), IsUnique = true)]
public class UserSurvey
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    
    public long SurveyId { get; set; }

    public long UserId { get; set; }

    [Precision(6, 1)] [Range(0, 100000)] public decimal Point { get; set; }

    public bool IsValid { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime Date { get; set; } = DateTime.UtcNow;

    [ForeignKey("UserId")] public virtual User User { get; set; } = null!;

    [ForeignKey("SurveyId")] public virtual Survey Survey { get; set; } = null!;
}