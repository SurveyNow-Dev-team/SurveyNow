using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

[PrimaryKey(nameof(QuestionDetailId), nameof(UserId))]
public class Answer
{
    public long QuestionDetailId { get; set; }
    
    public long UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;
    
}