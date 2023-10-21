using Application.DTOs.Request.User;
using Application.DTOs.Response;
using Application.DTOs.Response.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IOccupationService
    {
        Task<IEnumerable<FieldDTO>> GetFields();
        Task<IEnumerable<OccupationResponse>> GetOccupations();
        Task<OccupationResponse> GetOccupation(long id);
        Task CreateOccupation(OccupationRequest request);
        Task<OccupationResponse> UpdateOccupation(long id, OccupationRequest request);
    }
}
