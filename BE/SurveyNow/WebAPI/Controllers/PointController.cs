using Application.DTOs.Request;
using Application.DTOs.Request.Point;
using Application.DTOs.Response;
using Application.DTOs.Response.Point;
using Application.ErrorHandlers;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace SurveyNow.Controllers
{
    [Route("api/v1/points")]
    [ApiController]
    public class PointController : ControllerBase
    {
        private readonly IPointService _pointService;
        private readonly IUserService _userService;
        private readonly ILogger<PointController> _logger;

        public PointController(IPointService pointService, IUserService userService, ILogger<PointController> logger)
        {
            _pointService = pointService;
            _userService = userService;
            _logger = logger;
        }

        #region Purchase

        [HttpGet("purchase/history/{id}")]
        public async Task<ActionResult<PointPurchaseDetailResponse>> GetPointPurchaseDetailAsync(long id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID");
            }
            try
            {
                var pointDetail = await _pointService.GetPointPurchaseDetailAsync(id);
                if (pointDetail == null)
                {
                    throw new NotFoundException();
                }
                return Ok(pointDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving point purchase detail");
                return StatusCode(500, "An unexpected error occurred");
            }
        }

        [HttpGet("purchase/history")]
        public async Task<ActionResult<PagingResponse<PointPurchaseResponse>>> GetPointPurchasesFilteredAsync([FromQuery]PointDateFilterRequest dateFilter, [FromQuery]PointValueFilterRequest valueFilter, [FromQuery]PointSortOrderRequest sortOrder, [FromQuery]PagingRequest pagingRequest)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                if(user != null)
                {
                    var result = await _pointService.GetPointPurchasesFilteredAsync(dateFilter, valueFilter, sortOrder, pagingRequest, user.Id);
                    if (result != null)
                    {
                        return Ok(result);
                    }
                    return NotFound();
                    
                }
                else
                {
                    return BadRequest(new BadRequestException("Cannot retrieve user's point purchase history without user information!"));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving point purchase history");
                return StatusCode(500, "An unexpected error occurred");
            }
            throw new BadRequestException();
        }
        #endregion

        #region Redeem
        [HttpGet("redeem/history/{id}")]
        public async Task<ActionResult<PointRedeemDetailResponse>> GetPointRedeemDetailAsync(long id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID");
            }
            try
            {
                var pointDetail = await _pointService.GetPointRedeemDetailAsync(id);
                if (pointDetail == null)
                {
                    throw new NotFoundException();
                }
                return Ok(pointDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving point purchase detail");
                return StatusCode(500, "An unexpected error occurred");
            }
        }
        #endregion

    }
}
