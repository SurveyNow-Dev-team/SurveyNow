using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request.Survey;

public record AnswerOptionRequest
{
    [Required(ErrorMessage = "Row order can not be null.")]
    [Range(1, 20, ErrorMessage = "Row order must be between 1 and 20.")]
    public int RowOrder { get; init; }

    [Range(1, 20, ErrorMessage = "Column order must be between 1 and 20.")]
    public int? ColumnOrder { get; init; }
}