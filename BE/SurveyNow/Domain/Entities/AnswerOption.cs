using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class AnswerOption
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Range(1, 20)] public int RowOrder { get; set; }

    [Range(1, 20)] public int? ColumnOrder { get; set; }
    
    [Column(TypeName = "nvarchar(1000)")]
    public string? Content { get; set; }

    public long AnswerId { get; set; }

    [ForeignKey(nameof(AnswerId))] public virtual Answer Answer { get; set; } = null!;
}