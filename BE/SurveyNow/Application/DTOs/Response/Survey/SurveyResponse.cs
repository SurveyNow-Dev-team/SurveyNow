using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.DTOs.Response.Survey;

public class SurveyResponse
{
    public long Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int TotalQuestion { get; set; } = 0;

    public int TotalAnswer { get; set; } = 0; //store the number of users that did the survey

    public int TotalValidAnswer { get; set; } = 0; //store the number of valid users that did the survey

    public SurveyStatus Status { get; set; }

    public bool IsDelete { get; set; }

    public PackType? PackType { get; set; }

    public decimal? Point { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime? StartDate { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime? ExpiredDate { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

    public long CreatedUserId { get; set; }

    [ForeignKey(nameof(CreatedUserId))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public virtual Domain.Entities.User CreatedBy { get; set; } = null!;

    public ICollection<SectionResponse> Sections { get; set; } = new List<SectionResponse>();
}