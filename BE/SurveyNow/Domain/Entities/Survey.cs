﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class Survey
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Column(TypeName = "nvarchar(100)")]
    public string Name { get; set; } = null!;
    
    [Column(TypeName = "nvarchar(1000)")]
    public string? Description { get; set; }

    [Range(0, 1000)] public int TotalQuestion { get; set; } = 0;

    [Range(0, 100000)] public int TotalAnswer { get; set; } = 0; //store the number of users that did the survey

    [Range(0, 100000)] public int TotalValidAnswer { get; set; } = 0;//store the number of valid users that did the survey

    public SurveyStatus Status { get; set; } = SurveyStatus.Draft;

    public bool IsDelete { get; set; }
    
    public PackType? PackType { get; set; }
    
    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime? StartDate { get; set; }
    
    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime? ExpiredDate { get; set; }

    //need to check here
    [Precision(6, 1)] [Range(0, 100000)] public decimal? Point { get; set; } 
    
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
    public virtual User CreatedBy { get; set; } = null!;

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual ICollection<UserSurvey> UserSurveys { get; set; } = new List<UserSurvey>();
}