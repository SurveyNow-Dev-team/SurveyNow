using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request
{
    public class AddressRequest
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Detail is required")]
        public string Detail { get; set; }

        [Required(ErrorMessage = "Province is required")]
        public long ProvinceId { get; set; }

        [Required(ErrorMessage = "City is required")]
        public long CityId { get; set; }

        [Required(ErrorMessage = "District is required")]
        public long DistrictId { get; set; }
    }
}
