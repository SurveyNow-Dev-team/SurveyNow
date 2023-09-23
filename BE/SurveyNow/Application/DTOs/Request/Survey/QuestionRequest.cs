using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;
using Domain.Enums;

namespace Application.DTOs.Request.Survey;

public class QuestionRequest
{
    [Required (ErrorMessage = "Question order is required.")]
    [Range(1, 100, ErrorMessage = "Question order must be greater than 0.")]
    public int Order { get; set; }

    [Required(ErrorMessage = "Question type is required.")]
    public QuestionType Type { get; set; }

    public bool IsRequire { get; set; }

    public MultipleOptionType? MultipleOptionType { get; set; }

    [Range(1, 20, ErrorMessage = "Limit number must greater than 0 and can not exceed 20.")]
    public int? LimitNumber { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(500, ErrorMessage = "Title can not exceed 500 character.")]
    public string Title { get; set; } = null!;

    [StringLength(100, ErrorMessage = "Resource url is too long.")]
    public string? ResourceUrl { get; set; }

    //Need to check validate here
    public ICollection<RowOptionRequest>? RowOptions { get; set; } = new List<RowOptionRequest>();

    public ICollection<ColumnOptionRequest>? ColumnOptions { get; set; } = new List<ColumnOptionRequest>();
}