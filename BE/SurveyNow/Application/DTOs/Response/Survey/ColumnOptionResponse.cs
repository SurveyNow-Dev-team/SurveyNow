namespace Application.DTOs.Response.Survey;

public class ColumnOptionResponse
{
    public long QuestionId { get; set; }

    public int Order { get; set; }
    
    public string? Content { get; set; }
}