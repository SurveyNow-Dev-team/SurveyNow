using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request.Survey;

public class SurveyQuestionDetailRequest
{

    public bool IsCustom { get; set; }

    [StringLength(500, ErrorMessage = "Content can not exceed 500 characters. ")]
    public string? Content { get; set; }
}