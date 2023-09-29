namespace Application.DTOs.Response.Survey;

public class SectionResponse
{
    public int Order { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int TotalQuestion { get; set; }

    public ICollection<QuestionResponse> Questions { get; set; } = new List<QuestionResponse>();
}