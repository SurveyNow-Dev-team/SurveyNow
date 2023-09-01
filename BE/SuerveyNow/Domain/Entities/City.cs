using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class City
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    
    [Column(TypeName = "nvarchar(100)")]
    public string Name { get; set; }

    public virtual Province Province { get; set; } = null!;

    public virtual ICollection<District> Districts { get; } = new List<District>();
}