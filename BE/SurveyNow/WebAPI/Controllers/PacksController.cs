using Application.DTOs.Request.Pack;
using Application.DTOs.Response.Pack;
using Application.Interfaces.Services;
using Application.Utils;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace SurveyNow.Controllers
{
    [Route("api/v1/packs")]
    [ApiController]
    public class PacksController : ControllerBase
    {
        private readonly IPackService _packService;
        private readonly ILogger<PacksController> _logger;

        public PacksController(IPackService packService, ILogger<PacksController> logger)
        {
            _packService = packService;
            _logger = logger;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<PackInformation>>> GetAllPacksAsync()
        {
            try
            {
                return Ok(BusinessData.Packs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to retrieve all packs data");
                return StatusCode(500, "An error occurred while trying to retrieve all packs data");
            }
        }

        // Test end-point
        [HttpGet("calculate")]
        public async Task<ActionResult<decimal>> CalculatePackPriceAsync([FromQuery]PackType packType, [FromQuery]int participants)
        {
            return await _packService.CalculatePackPriceAsync(packType, participants);
        }

        [HttpGet("recommend")]
        public async Task<ActionResult<List<PackInformation>>> GetRecommendedPacksAsync([FromQuery]PackRecommendRequest recommendRequest)
        {
            if(recommendRequest == null || recommendRequest.TotalQuestions <= 0)
            {
                return BadRequest("Invalid request!");
            }
            try
            {
                return await _packService.GetRecommendedPacksAsync(recommendRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to retrieve recommeded packs data");
                return StatusCode(500, "An error occurred while trying to retrieve recommeded packs data");
            }
        }
    }
}
