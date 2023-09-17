namespace Application.DTOs.Response.Survey;

public class RowOptionResponse
{
    public long QuestionId { get; set; }

    public int Order { get; set; }

    public bool IsCustom { get; set; }

    public int TotalChoose { get; set; } = 0;

    public string? Content { get; set; }
}