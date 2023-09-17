using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request.Survey;

public class RowOptionRequest
{
    [Required (ErrorMessage = "Row option order is required.")]
    [Range(1, 20, ErrorMessage = "Order must be greater than 0 and can not exceed 20.")]
    public int Order { get; set; }

    //For Other choice type
    public bool IsCustom { get; set; }

    [StringLength(500, ErrorMessage = "Content can not exceed 500 character.")]
    public string? Content { get; set; }
}