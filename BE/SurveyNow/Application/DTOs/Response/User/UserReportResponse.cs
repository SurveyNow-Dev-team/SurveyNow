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

namespace Application.DTOs.Response.User
{
    public class UserReportResponse
    {
        public long Id { get; set; }

        public string? Type { get; set; }

        public string Reason { get; set; } = null!;

        public string Status { get; set; }

        public string? Result { get; set; }

        public long CreatedUserId { get; set; }

        public long? UserId { get; set; }

        public long? SurveyId { get; set; }

    }
}
