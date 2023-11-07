using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request.User
{
    public class UserReportStatusRequest
    {
        public string Status { get; set; }

        public string? Result { get; set; }
    }
}
