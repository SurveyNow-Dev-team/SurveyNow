using Application.DTOs.Request.User;
using Application.DTOs.Response.User;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SurveyNow.Controllers
{
    [Route("api/v1/occupations")]
    [ApiController]
    public class OccupationsController : ControllerBase
    {
        private readonly IOccupationService _service;

        public OccupationsController(IOccupationService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<OccupationResponse>> GetOccupations()
        {
            var occupations = await _service.GetOccupations();
            return occupations;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<OccupationResponse> GetOccupation(long id)
        {
            var occupation = await _service.GetOccupation(id);
            return occupation;
        }

        [HttpPut]
        [Authorize]
        public async Task<OccupationResponse> UpdateOccupation(long id, OccupationRequest request)
        {
            var occupation = await _service.UpdateOccupation(id, request);
            return occupation;
        }

        [HttpPost]
        [Authorize]
        public async Task CreateOccupation(OccupationRequest occupationRequest)
        {
            await _service.CreateOccupation(occupationRequest);
        }
    }
}
