using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(QuestionId), nameof(Order), IsUnique = true)]
public class RowOption
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public long QuestionId { get; set; }

    [Range(1, 20)] public int Order { get; set; }

    public bool IsCustom { get; set; }

    public int TotalChoose { get; set; } = 0;

    [Column(TypeName = "nvarchar(500)")] public string? Content { get; set; }

    [ForeignKey(nameof(QuestionId))] public virtual Question Question { get; set; } = null!;
}