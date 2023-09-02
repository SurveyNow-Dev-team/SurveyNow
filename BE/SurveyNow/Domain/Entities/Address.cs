using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Address
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Column(TypeName = "nvarchar(100)")] public string Detail { get; set; } = null!;
    
    public virtual Province? Province { get; set; } //need to be optional to prevent cascade delete
    
    public virtual City? City { get; set; }
    
    public virtual District? District { get; set; }
    
}