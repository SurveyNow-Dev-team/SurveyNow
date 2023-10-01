using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.DTOs.Request.Survey;

public class SurveyRequest
{
    [Required(ErrorMessage = "Survey title is required.")]
    [StringLength(300, ErrorMessage = "Title can not exceed 300 character.")]public string Title { get; set; } = null!;

    [StringLength(3000, ErrorMessage = "Description can not exceed 3000 character.")]
    public string? Description { get; set; }

    // public PackType? PackType { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
    [Precision(2)]
    public DateTime? StartDate { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
    [Precision(2)]
    public DateTime? ExpiredDate { get; set; }

    public virtual ICollection<SectionRequest> Sections { get; set; } = new List<SectionRequest>();
}