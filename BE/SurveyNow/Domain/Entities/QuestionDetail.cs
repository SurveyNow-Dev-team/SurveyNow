using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

// [PrimaryKey(nameof(SurveyId), nameof(QuestionOrder), nameof(DetailOrder))]
[Index(nameof(SurveyId), nameof(QuestionOrder), nameof(DetailOrder), IsUnique = true)]
public class QuestionDetail
{
    [Key]
    public long Id { get; set; }
    
    // [ForeignKey("Question")]
    public long SurveyId { get; set; }
    
    // [ForeignKey("Question")]
    public int QuestionOrder { get; set; }
    
    [Range(1, 20)]
    public int DetailOrder { get; set; }

    public bool IsCustom { get; set; }

    [Range(0, int.MaxValue)] 
    public int TotalAnswer { get; set; } = 0;
    
    [Column(TypeName = "nvarchar(500)")] 
    public string? Content { get; set; }

    public virtual Question Question { get; set; } = null!;

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
}