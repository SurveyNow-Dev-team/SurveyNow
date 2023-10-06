using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response.User
{
    public class OccupationResponse
    {
        public long? Id { get; set; }

        public decimal? Income { get; set; }

        public string? PlaceOfWork { get; set; } = null!;

        public string? Currency { get; set; } = null!;

        public FieldDTO? Field { get; set; }
    }
}
