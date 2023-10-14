using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Application.DTOs.Request.Survey;

public class PostSurveyRequest
{
    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
    [Precision(2)]
    public DateTime? StartDate { get; init; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
    [Precision(2)]
    [Required(ErrorMessage = "Expired date is required.")]
    public DateTime ExpiredDate { get; init; }
}