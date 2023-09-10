using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class AddressResponse
    {
        public long Id { get; set; }

        public string Detail { get; set; }

        public ProvinceResponse? Province { get; set; }

        public CityResponse? City { get; set; }

        public DistrictResponse? District { get; set; }
    }
}
