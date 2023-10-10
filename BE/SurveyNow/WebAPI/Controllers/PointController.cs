using Application.DTOs.Request;
using Application.DTOs.Request.Momo;
using Application.DTOs.Request.Point;
using Application.DTOs.Response;
using Application.DTOs.Response.Momo;
using Application.DTOs.Response.Point;
using Application.DTOs.Response.Point.History;
using Application.ErrorHandlers;
using Application.Interfaces.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Xem chi tiết thông tin biến động điểm của người dùng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Lấy danh sách biến động điểm của người dùng hiện tại,
        /// có thể filter theo ngày, giá trị điểm và cách sắp xếp
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dateFilter"></param>
        /// <param name="valueFilter"></param>
        /// <param name="sortOrder"></param>
        /// <param name="pagingRequest"></param>
        /// <returns></returns>
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
        //[HttpPost("do-survey/")]
        //public async Task<ActionResult<BasePointHistoryResponse>> AddPointDoSurveyAsync([FromQuery] decimal pointAmount, [FromQuery] long surveyId)
        //{
        //    try
        //    {
        //        var user = await _userService.GetCurrentUserAsync();
        //        var result = await _pointService.AddDoSurveyPointAsync(user.Id, surveyId, pointAmount);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred");
        //        return StatusCode(500, "An error occurred");
        //    }
        //}
        /// <summary>
        /// Gửi yêu cầu nạp điểm vào tài khoản của người dùng.
        /// Yêu cầu sẽ được xử lý và trả về các phương thức thanh toán bằng momo nếu xử lý thành công
        /// </summary>
        /// <param name="purchaseRequest"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Nhận và xử lý kết quả thanh toán giao dịch nạp điểm bằng momo
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("purchase/momo/return")]
        public async Task<ActionResult> OnReceivingMomoTransactionResult([FromQuery] MomoCreatePaymentResultRequest payload, [FromQuery] long userId)
        {
            _logger.LogInformation("Receive momo transaction result from client");
            var result = await _pointService.ProcessMomoPaymentResultAsync(userId, payload);
            return Ok(result);
        }

        /// <summary>
        /// Gửi yêu cầu đổi điểm thành tiền mặt với điểm đến là tài khoản momo của người dùng
        /// </summary>
        /// <param name="redeemRequest"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("redeem/momo")]
        public async Task<ActionResult<PointCreateRedeemOrderResponse>> CreateMomoGiftRedeemOrder([FromBody] PointRedeemRequest redeemRequest)
        {
            var result = await _pointService.ProcessCreateGiftRedeemOrderAsync(redeemRequest);
            return Ok(result);
        }
    }
}
