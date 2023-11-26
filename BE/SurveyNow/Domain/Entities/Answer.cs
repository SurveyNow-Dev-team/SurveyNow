using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(QuestionId), nameof(UserId), IsUnique = true)]
public class Answer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public long QuestionId { get; set; }

    public long UserId { get; set; }

    [Column(TypeName = "nvarchar(1000)")] public string? Content { get; set; }

    public int? RateNumber { get; set; }

    [JsonIgnore]
    [ForeignKey(nameof(QuestionId))] public virtual Question Question { get; set; } = null!;

    [ForeignKey(nameof(UserId))] public virtual User User { get; set; } = null!;

    public virtual ICollection<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>();
}