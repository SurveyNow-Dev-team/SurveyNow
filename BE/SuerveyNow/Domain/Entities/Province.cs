using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Province
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    
    [Column(TypeName = "nvarchar(100)")]
    public string Name { get; set; }

    public virtual ICollection<City> Cities { get; } = new List<City>();
}