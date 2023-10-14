namespace Application.DTOs.Response.Survey;

public class AnswerOptionResponse
{
    public int RowOrder { get; set; }

    public int? ColumnOrder { get; set; }

    public string? Content { get; set; }
}