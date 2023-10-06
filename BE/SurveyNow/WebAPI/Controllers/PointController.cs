using Application.DTOs.Request;
using Application.DTOs.Request.Momo;
using Application.DTOs.Request.Point;
using Application.DTOs.Response;
using Application.DTOs.Response.Momo;
using Application.DTOs.Response.Point.History;
using Application.ErrorHandlers;
using Application.Interfaces.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Twilio.Jwt.Taskrouter;

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

        [Authorize]
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

        [Authorize]
        [HttpGet("history")]
        public async Task<ActionResult<PagingResponse<ShortPointHistoryResponse>>> GetPointPurchasesFilteredAsync([FromQuery] PointHistoryType type, [FromQuery] PointDateFilterRequest dateFilter, [FromQuery] PointValueFilterRequest valueFilter, [FromQuery] PointSortOrderRequest sortOrder, [FromQuery] PagingRequest pagingRequest)
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

        // Test end-point
        [HttpPost("do-survey/")]
        public async Task<ActionResult<BasePointHistoryResponse>> AddPointDoSurveyAsync([FromQuery] decimal pointAmount, [FromQuery] long surveyId)
        {
            try
            {
                var user = await _userService.GetCurrentUserAsync();
                var result = await _pointService.AddDoSurveyPointAsync(user.Id, surveyId, pointAmount);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred");
                return StatusCode(500, "An error occurred");
            }
        }

        [Authorize]
        [HttpPost("purchase/momo")]
        public async Task<ActionResult<MomoPaymentMethodResponse>> CreateMomoPointPurchaseOrder([FromBody] PointPurchaseRequest purchaseRequest)
        {
            var user = await _userService.GetCurrentUserAsync();
            if (user == null)
            {
                return Unauthorized("Cannot retreive user's identity");
            }
            try
            {
                var result = await _pointService.CreateMomoPurchasePointOrder(user, purchaseRequest);
                if (result == null)
                {
                    return NotFound("Failed to retrieve momo payment method");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred when trying to create point purchase order with Momo");
                return StatusCode(500, "An exception occurred when trying to create point purchase order with Momo");
            }
        }

        //[Authorize]
        [HttpGet]
        [Route("purchase/momo/return")]
        public async Task<ActionResult> OnReceivingMomoTransactionResult([FromQuery] MomoCreatePaymentResultRequest payload, [FromQuery] long userId)
        {
            _logger.LogInformation("Receive momo transaction result from client");
            var result = await _pointService.ProcessMomoPaymentResultAsync(userId, payload);
            return Ok(result);
        }
    }
}
