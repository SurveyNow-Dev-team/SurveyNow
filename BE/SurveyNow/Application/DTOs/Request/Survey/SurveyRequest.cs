using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.DTOs.Request.Survey;

public class SurveyRequest
{
    [Column(TypeName = "nvarchar(300)")] public string Title { get; set; } = null!;

    [Column(TypeName = "nvarchar(3000)")] public string? Description { get; set; }

    public PackType? PackType { get; set; }

    //need to check here
    [Precision(6, 1)] [Range(0, 100000)] public decimal? Point { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime? StartDate { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime? ExpiredDate { get; set; }

    public virtual ICollection<SectionRequest> Sections { get; set; } = new List<SectionRequest>();
}