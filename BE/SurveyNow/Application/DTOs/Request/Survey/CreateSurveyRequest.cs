using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.Request.Survey;

public class CreateSurveyRequest
{

    [Required (ErrorMessage = "Survey name is required!")]
    public string Name { get; set; } = null!;
    
    [StringLength(1000, ErrorMessage = "The description can not exceed 1000 characters.")]
    public string? Description { get; set; }
    
    public SurveyStatus Status { get; set; }

    public PackType? PackType { get; set; }
    
    public DateTime? StartDate { get; set; }
    
    public DateTime? ExpiredDate { get; set; }

    public ICollection<SurveyQuestionRequest> SurveyQuestionRequests { get; set; } = new List<SurveyQuestionRequest>();
}