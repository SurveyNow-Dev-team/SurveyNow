using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(SurveyId), nameof(Order), IsUnique = true)]
public class Section
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public long SurveyId { get; set; }

    [Range(1, 20)] public int Order { get; set; } = 1;

    [Column(TypeName = "nvarchar(300)")] public string? Title { get; set; }

    [Column(TypeName = "nvarchar(3000)")] public string? Description { get; set; }

    [Range(1, 100)] public int TotalQuestion { get; set; } = 1;

    [ForeignKey(nameof(SurveyId))] public virtual Survey Survey { get; set; } = null!;

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}