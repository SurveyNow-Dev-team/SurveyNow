using System.Collections;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.Request.Survey;

public class SurveyQuestionRequest
{
    [Required(ErrorMessage = "Order of question is required. ")]
    [Range(1, 1000)]
    public int Order { get; set; }

    [Required(ErrorMessage = "Question type is required. ")]
    public QuestionType Type { get; set; }

    public bool IsRequire { get; set; }

    [StringLength(500, ErrorMessage = "Title can not exceed 500 character. ")]
    public string Title { get; set; } = null!;

    //Need to allow user to upload resource here
    // public string? ResourceUrl { get; set; }

    public List<SurveyQuestionDetailRequest> SurveyQuestionDetailRequests { get; set; } =
        new List<SurveyQuestionDetailRequest>();
}