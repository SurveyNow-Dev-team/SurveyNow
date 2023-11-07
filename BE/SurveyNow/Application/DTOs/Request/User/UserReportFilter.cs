using Domain.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request.User
{
    public class UserReportFilter
    {
        public string? Type { get; set; }

        public string? Reason { get; set; } = null!;

        public UserReportStatus? Status { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        [Precision(2)]
        public DateTime? CreatedDate { get; set; }

        public string? Result { get; set; }

        public long? CreatedUserId { get; set; }

        public long? UserId { get; set; }

        public long? SurveyId { get; set; }
    }
}
