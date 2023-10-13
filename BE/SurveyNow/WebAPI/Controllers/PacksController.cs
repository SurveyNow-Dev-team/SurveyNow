using Application.DTOs.Request.Pack;
using Application.DTOs.Response.Pack;
using Application.Interfaces.Services;
using Application.Utils;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SurveyNow.Controllers
{
    /// <summary>
    /// Api for pack related resource
    /// </summary>
    [Route("api/v1/packs")]
    [ApiController]
    public class PacksController : ControllerBase
    {
        private readonly IPackService _packService;
        private readonly ILogger<PacksController> _logger;
        private readonly IUserService _userService;

        public PacksController(IPackService packService, ILogger<PacksController> logger, IUserService userService)
        {
            _packService = packService;
            _logger = logger;
            _userService = userService;
        }
        /// <summary>
        /// Lấy danh sách toàn bộ các gói dùng để đăng khảo sát
        /// </summary>
        /// <returns></returns>
        [Authorize]
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

        /// <summary>
        /// Tính số điểm cần để mua gói dựa trên loại gói và số người điền khảo sát
        /// </summary>
        /// <param name="packType"></param>
        /// <param name="participants"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("calculate")]
        public async Task<ActionResult<decimal>> CalculatePackPriceAsync([FromQuery] PackType packType, [FromQuery] int participants)
        {
            var result =  await _packService.CalculatePackPriceAsync(packType, participants);
            return Ok(result);
        }

        /// <summary>
        /// Get recommended pack(s) based on given survey's information
        /// </summary>
        /// <param name="recommendRequest"></param>
        /// <returns></returns>
        [Authorize]
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

        /// <summary>
        /// Xử lý yêu cầu mua gói để đăng khảo sát của người dùng
        /// </summary>
        /// <param name="purchaseRequest"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("purchase")]
        public async Task<ActionResult> ProcessPackPurchaseRequest(PackPurchaseRequest purchaseRequest)
        {
            var user = await _userService.GetCurrentUserAsync();
            if(user == null)
            {
                return Unauthorized("Cannot retreive user's identity");
            }
            try
            {
                await _packService.PurchasePackAsync(user, purchaseRequest);
                return Ok("Successfully purchase pack");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (OperationCanceledException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
