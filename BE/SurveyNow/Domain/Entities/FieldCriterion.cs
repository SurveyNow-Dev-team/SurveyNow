using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class FieldCriterion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public long FieldId { get; set; }

    [ForeignKey(nameof(FieldId))] public virtual Field Field { get; set; } = null!;
}