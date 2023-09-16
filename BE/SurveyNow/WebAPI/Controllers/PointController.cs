using Application.DTOs.Request;
using Application.DTOs.Request.Point;
using Application.DTOs.Response;
using Application.DTOs.Response.Point.History;
using Application.ErrorHandlers;
using Application.Interfaces.Services;
using Domain.Enums;
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

        [HttpGet("history/{id}")]
        public async Task<ActionResult<BasePointHistoryResponse>> GetPointHistoryDetailAsync(long id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid point history ID");
            }
            try
            {
                var pointHistoryDetail = await _pointService.GetPointHistoryDetailAsync(id);
                if (pointHistoryDetail == null)
                {
                    return NotFound("Cannot find point history detail with the given ID");
                }
                return Ok(pointHistoryDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to retrieve point history detail");
                return StatusCode(500, "An error occurred while trying to retrieve point history detail");
            }
        }

        [HttpGet("history")]
        public async Task<ActionResult<PagingResponse<ShortPointHistoryResponse>>> GetPointPurchasesFilteredAsync([FromQuery]PointHistoryType type, [FromQuery] PointDateFilterRequest dateFilter, [FromQuery] PointValueFilterRequest valueFilter, [FromQuery] PointSortOrderRequest sortOrder, [FromQuery] PagingRequest pagingRequest)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                if (user != null)
                {
                    var result = await _pointService.GetPaginatedPointHistoryListAsync(user.Id, type, dateFilter, valueFilter, sortOrder, pagingRequest);
                    if (result != null)
                    {
                        return Ok(result);
                    }
                    return NotFound();
                }
                else
                {
                    return BadRequest("Unspecified user ID! Cannot retrieve user's point history!");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to retrieve point history");
                return StatusCode(500, "An error occurred while trying to retrieve point history");
            }
        }
    }
}
