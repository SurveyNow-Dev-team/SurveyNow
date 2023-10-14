namespace Application.DTOs.Response.Survey;

public class UserSurveyResponse
{
    public long UserId { get; set; }

    public bool IsValid { get; set; }
    
    public string? Date { get; set; }

    public string FullName { get; set; } = string.Empty;
}