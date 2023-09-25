using Domain.Enums;

namespace Application.DTOs.Response.Survey;

public class SurveyDetailResponse
{
    public long Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int TotalQuestion { get; set; } = 0;

    public int TotalAnswer { get; set; } = 0; //store the number of users that did the survey

    public int TotalValidAnswer { get; set; } = 0; //store the number of valid users that did the survey

    public SurveyStatus Status { get; set; }

    public bool IsDelete { get; set; }

    public PackType? PackType { get; set; }

    public decimal? Point { get; set; }

    public string? StartDate { get; set; }

    public string? ExpiredDate { get; set; }

    public string? CreatedDate { get; set; }

    public string? ModifiedDate { get; set; }

    public long CreatedUserId { get; set; }

    public ICollection<SectionResponse> Sections { get; set; } = new List<SectionResponse>();
}