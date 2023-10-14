using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request.Survey;

public record AnswerRequest
{
    [Required (ErrorMessage = "Question id can not be null.")]
    public long QuestionId { get; set; }

    [StringLength(1000, ErrorMessage = "Content length can not exceed 1000 character.")]
    public string? Content { get; set; }

    [Range(1, 20, ErrorMessage = "Rate number can not exceed 20.")]
    public int? RateNumber { get; set; }

    public virtual ICollection<AnswerOptionRequest>? AnswerOptions { get; set; } = new List<AnswerOptionRequest>();
}