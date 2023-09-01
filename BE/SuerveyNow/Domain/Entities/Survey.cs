using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class Survey
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Column(TypeName = "nvarchar(100)")]
    public string Name { get; set; } = null!;
    
    [Column(TypeName = "nvarchar(1000)")]
    public string? Description { get; set; }
    
    [Range(0, 1000)]
    public int TotalQuestion { get; set; }
    
    public SurveyStatus Status { get; set; }

    public bool IsDelete { get; set; }
    
    public PackType? PackType { get; set; }
    
    [Precision(2)]
    public DateTime? ExpiredDate { get; set; }
    
    public int? Point { get; set; }
    
    [Precision(2)]
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    
    [Precision(2)]
    public DateTime ModifiedDate { get; set; } = DateTime.Now;

    public virtual User CreatedBy { get; set; } = null!;
}