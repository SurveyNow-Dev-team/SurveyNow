﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request.User
{
    public class OccupationRequest
    {

        public decimal Income { get; set; }

        public string PlaceOfWork { get; set; }

        public string? Currency { get; set; }

        public long? FieldId { get; set; }

        public long? PositionId { get; set; }
    }
}
