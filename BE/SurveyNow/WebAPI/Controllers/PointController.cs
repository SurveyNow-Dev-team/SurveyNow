﻿using Application.DTOs.Request;
using Application.DTOs.Request.Momo;
using Application.DTOs.Request.Point;
using Application.DTOs.Request.Transaction;
using Application.DTOs.Response;
using Application.DTOs.Response.Momo;
using Application.DTOs.Response.Point;
using Application.DTOs.Response.Point.History;
using Application.DTOs.Response.Transaction;
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
                throw new BadRequestException("Id của lịch sử biến động điểm phải lớn hơn 0");
            }
            var pointHistoryDetail = await _pointService.GetPointHistoryDetailAsync(id);
            if (pointHistoryDetail == null)
            {
                throw new NotFoundException($"Không tìm thấy lịch sử biến động điểm với Id: {id}");
            }
            return Ok(pointHistoryDetail);
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
            var user = await _userService.GetCurrentUserAsync();
            if (user == null)
            {
                throw new NotFoundException($"Không tìm thấy thông tin của người dùng");
            }
            var result = await _pointService.GetPaginatedPointHistoryListAsync(user.Id, type, dateFilter, valueFilter, sortOrder, pagingRequest);
            if (result != null)
            {
                return Ok(result);
            }
            return new PagingResponse<ShortPointHistoryResponse>()
            {
                Results = Enumerable.Empty<ShortPointHistoryResponse>().ToList(),
            };
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
                throw new NotFoundException("Không tìm thấy thông tin của người dùng");
            }
            var result = await _pointService.CreateMomoPurchasePointOrder(user, purchaseRequest);
            if (result == null)
            {
                throw new NotFoundException("Không thể tạo phương thức thanh toán bằng Momo");
            }
            return Ok(result);

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
        /// Gửi yêu cầu đổi điểm thành tiền mặt với điểm đến là tài khoản (số điện thoại) ví điện tử của người dùng
        /// </summary>
        /// <param name="redeemRequest"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("redeem/request")]
        public async Task<ActionResult<PointCreateRedeemOrderResponse>> CreateMomoGiftRedeemOrder([FromBody] PointRedeemRequest redeemRequest)
        {
            var result = await _pointService.ProcessCreateGiftRedeemOrderAsync(redeemRequest);
            return Ok(result);
        }

        /// <summary>
        /// Người dùng gửi yêu cầu nạp điểm với số điểm và hình thức thanh toán.
        /// Sau khi tạo yêu cầu thành công, người dùng cần phải chuyển tiền tới tài khoản của App SurveyNow đúng với thông tin được App trả vể.
        /// Sau khi yêu cầu nạp điểm được tạo và người dùng đã chuyển đủ số tiền, App SurveyNow sẽ xử lý yêu cầu giao dịch của người dùng.
        /// </summary>
        /// <param name="purchaseRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("purchase/request")]
        public async Task<ActionResult<PointPurchaseTransactionCreateResponse>> CreatePointPurchaseRequest([FromBody] PointPurchaseTransactionCreateRequest purchaseRequest)
        {
            var user = await _userService.GetCurrentUserAsync();
            if (user == null)
            {
                throw new NotFoundException($"Không tìm thấy thông tin của người dùng");
            }
            var result = await _pointService.CreatePointPurchaseRequest(user, purchaseRequest);
            return Ok(result);
        }
    }
}
