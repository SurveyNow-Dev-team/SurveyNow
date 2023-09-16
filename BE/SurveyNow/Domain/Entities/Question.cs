using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(SectionId), nameof(Order), IsUnique = true)]
public class Question
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public long SectionId { get; set; }

    [Range(1, 100)] public int Order { get; set; }

    public QuestionType Type { get; set; }

    public bool IsRequire { get; set; }

    public int TotalAnswer { get; set; } = 0;

    public MultipleOptionType? MultipleOptionType { get; set; }

    [Range(1, 20)] public int? LimitNumber { get; set; }

    [Column(TypeName = "nvarchar(500)")] public string Title { get; set; } = null!;

    [Column(TypeName = "nvarchar(100)")] public string? ResourceUrl { get; set; }

    [ForeignKey(nameof(SectionId))] public virtual Section Section { get; set; } = null!;

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual ICollection<ColumnOption> ColumnOptions { get; set; } = new List<ColumnOption>();

    public virtual ICollection<RowOption> RowOptions { get; set; } = new List<RowOption>();
}