using Domain.Enums;

namespace Application.DTOs.Response.Survey;

public class QuestionResponse
{
    public long Id { get; set; }

    public int Order { get; set; }

    public QuestionType Type { get; set; }

    public bool IsRequire { get; set; }

    public int TotalAnswer { get; set; }

    public MultipleOptionType? MultipleOptionType { get; set; }

    public int? LimitNumber { get; set; }

    public string Title { get; set; } = null!;

    public string? ResourceUrl { get; set; }

    public ICollection<RowOptionResponse> RowOptions { get; set; } = new List<RowOptionResponse>();

    public ICollection<ColumnOptionResponse> ColumnOptions { get; set; } = new List<ColumnOptionResponse>();
    public ICollection<AnswerResponse>? Answers { get; set; }
}