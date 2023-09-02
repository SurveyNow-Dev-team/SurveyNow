using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[PrimaryKey(nameof(QuestionDetailId), nameof(UserId))]
public class Answer
{
    public long QuestionDetailId { get; set; } 
    
    public long UserId { get; set; }
    
    public string? Content { get; set; }

    [ForeignKey(nameof(QuestionDetailId))]
    public virtual QuestionDetail QuestionDetail { get; set; } = null!;
    
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;
}