using Domain.Enums;

namespace Application.DTOs.Response.Survey;

public class CommonSurveyResponse
{
    public long Id { get; set; }

    public string Title { get; set; } = null!;

    public int TotalQuestion { get; set; } = 0;

    public SurveyStatus? Status { get; set; }

    //need to check here
    public decimal? Point { get; set; }

    public string? StartDate { get; set; }

    public string? ExpiredDate { get; set; }

    public long CreatedUserId { get; set; }
    
    public string CreatedUserFullName { get; set; } = null!;
}