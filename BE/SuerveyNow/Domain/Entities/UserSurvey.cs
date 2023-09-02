using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[PrimaryKey(nameof(SurveyId), nameof(UserId))]
public class UserSurvey
{
    public long SurveyId { get; set; }

    public long UserId { get; set; }

    [Precision(6, 1)]
    [Range(0, 100000)]
    public decimal Point { get; set; }

    public bool IsValid { get; set; }

    public bool IsModified { get; set; }

    [Precision(2)]
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    [Precision(2)]
    public DateTime? ModifiedDate { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    [ForeignKey("SurveyId")]
    public virtual Survey Survey { get; set; } = null!;
}