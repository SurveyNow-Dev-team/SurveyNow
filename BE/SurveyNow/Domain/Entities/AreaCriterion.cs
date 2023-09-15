using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class AreaCriterion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public long ProvinceId { get; set; }

    [ForeignKey(nameof(ProvinceId))] public virtual Province Province { get; set; } = null!;
}