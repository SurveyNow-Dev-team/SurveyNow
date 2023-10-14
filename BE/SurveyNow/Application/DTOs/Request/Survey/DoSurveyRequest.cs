using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request.Survey;

public class DoSurveyRequest
{
    [Required(ErrorMessage = "Survey id can not be null.")]
    public int SurveyId { get; init; }

    public ICollection<AnswerRequest> Answers { get; set; } = new List<AnswerRequest>();
}