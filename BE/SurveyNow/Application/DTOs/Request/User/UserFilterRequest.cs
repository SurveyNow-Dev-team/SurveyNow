﻿using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request.User
{
    public class UserFilterRequest
    {
        public string? FullName { get; set; } = null!;
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [RegularExpression(@"\d+")]
        public string PhoneNumber { get; set; }
        public string? Status { get; set; }

        public string? Role { get; set; }
        public RelationshipStatus? RelationshipStatus { get; set; }
        [EmailAddress]
        public string? Email { get; set; }

        public string? LangKey { get; set; }

        public string? Currency { get; set; }
    }
}