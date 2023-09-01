using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Hobby
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    
    [Column(TypeName = "nvarchar(50)")]
    public string Name { get; set; }

    public virtual User User { get; set; } = null!;
}