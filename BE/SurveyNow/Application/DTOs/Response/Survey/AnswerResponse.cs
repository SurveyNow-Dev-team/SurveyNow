namespace Application.DTOs.Response.Survey;

public class AnswerResponse
{
    public string? Content { get; set; }

    public int? RateNumber { get; set; }

    public virtual ICollection<AnswerOptionResponse?> AnswerOptions { get; set; } = new List<AnswerOptionResponse?>();
}