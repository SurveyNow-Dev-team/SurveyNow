using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class Occupation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Precision(11,2)]
    public decimal Income { get; set; }
    
    [Column(TypeName = "nvarchar(250)")]
    public string PlaceOfWork { get; set; } = null!;
    
    [Column(TypeName = "nvarchar(10)")]
    public string Currency { get; set; } = null!;
    
    public virtual Field? Field { get; set; }
    
    public virtual Position? Position { get; set; }
}