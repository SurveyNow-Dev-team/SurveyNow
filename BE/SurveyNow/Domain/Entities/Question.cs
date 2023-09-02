using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[PrimaryKey(nameof(SurveyId), nameof(Order))]
public class Question
{
    public long SurveyId { get; set; }
    
    [Range(0, 1000)]
    public int Order { get; set; } 
    
    public QuestionType Type { get; set; }

    public bool IsRequire { get; set; } 

    [Column (TypeName = "nvarchar(500)")]
    public string Title { get; set; } = null!;

    public string? ResourceUrl { get; set; }
    
    [ForeignKey("SurveyId")] 
    public virtual Survey Survey { get; set; } = null!;

    public virtual ICollection<QuestionDetail> QuestionDetails { get; set; } = new List<QuestionDetail>();
}