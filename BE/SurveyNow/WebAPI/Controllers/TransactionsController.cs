using Application.DTOs.Request;
using Application.DTOs.Request.Transaction;
using Application.DTOs.Response;
using Application.DTOs.Response.Transaction;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SurveyNow.Controllers
{
    /// <summary>
    /// Processing user transaction (redeem point,...)
    /// </summary>
    [Route("api/v1/transaction")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IPointService _pointService;
        private readonly IUserService _userService;
        private readonly ITransactionService _transactionService;
        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(IPointService pointService, IUserService userService, ILogger<TransactionsController> logger, ITransactionService transactionService)
        {
            _pointService = pointService;
            _userService = userService;
            _logger = logger;
            _transactionService = transactionService;
        }

        /// <summary>
        /// Lấy danh sách các giao dịch đổi điểm đang chờ được xử lý, sắp xếp theo ngày tạo tăng dần (cũ nhất -> mới nhất)
        /// </summary>
        /// <param name="pagingRequest"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("point-redeem/pending")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PagingResponse<TransactionResponse>>> GetPaginatedPendingRedeemTransaction([FromQuery] PagingRequest pagingRequest)
        {
            var result = await _transactionService.GetPaginatedPendingRedeemTransactionsAsync(pagingRequest);
            return Ok(result);
        }

        /// <summary>
        /// Lấy thông tin chi tiết của giao dịch điểm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:long}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<TransactionResponse>> GetTransaction([FromRoute] long id)
        {
            var result = await _transactionService.GetTransactionsAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Hủy giao dịch đổi điểm đang chờ xử lý và hoàn lại điểm vào tài khoản của người dùng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("point-redeem/{id:long}/cancel")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProccessRedeemTransactionResult>> CancelRedeemTransaction([FromRoute] long id)
        {
            var result = await _transactionService.CancelRedeemTransaction(id);
            return Ok(result);
        }

        /// <summary>
        /// Xử lý giao dịch đổi điểm của người dùng sau khi đã chuyển tiền đến ví momo của người dùng thành công.
        /// Cần cung cấp mã giao dịch momo
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("point-redeem/{id:long}/process")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProccessRedeemTransactionResult>> ProcessRedeemTransaction([FromRoute] long id, [FromBody] UpdatePointRedeemTransactionRequest request)
        {
            var result = await _transactionService.ProcessRedeemTransaction(id, request);
            return Ok(result);
        }

        /// <summary>
        /// Xem danh sách lịch sử giao dịch của hệ thống. Có thể lọc theo các tiêu chí (loại giao dịch, trạng thái, thời gian) và thứ tự sắp xếp
        /// </summary>
        /// <param name="historyRequest"></param>
        /// <param name="pagingRequest"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("history")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PagingResponse<TransactionResponse>>> GetTransactionHistoryWithFilter([FromQuery] TransactionHistoryRequest historyRequest, [FromQuery] PagingRequest pagingRequest)
        {
            var result = await _transactionService.GetTransactionHistory(pagingRequest, historyRequest);
            return Ok(result);
        }

        /// <summary>
        /// Lấy danh sách các giao dịch nạp điểm đang chờ được xử lý, sắp xếp theo ngày tạo tăng dần (cũ nhất -> mới nhất). Có thể tìm kiếm theo Id của của giao dịch
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pagingRequest"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("point-purchase/pending")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PagingResponse<TransactionResponse>>> GetPaginatedPendingPurchaseTransaction([FromQuery] long? id, [FromQuery] PagingRequest pagingRequest)
        {
            var result = await _transactionService.GetPaginatedPendingPurchaseTransactionsAsync(id, pagingRequest);
            return Ok(result);
        }

        /// <summary>
        /// Hủy giao dịch nạp điểm đang chờ xử lý của người dùng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("point-purchase/{id:long}/cancel")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProccessRedeemTransactionResult>> CancelPurchaseTransaction([FromRoute] long id)
        {
            var result = await _transactionService.CancelPurchaseTransaction(id);
            return Ok(result);
        }

        /// <summary>
        /// Xử lý giao dịch nạp điểm của người dùng sau khi đã nhận được giao dịch thanh toán đầy đủ qua ví điện tử của người dùng.
        /// Tài khoản của người dùng sẽ được cập nhật số điểm tương ứng với số điểm trong yêu cầu giao dịch nạp điểm
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("point-purchase/{id:long}/process")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProccessRedeemTransactionResult>> ProcessPurchaseTransaction([FromRoute] long id, [FromBody] UpdatePointPurchaseTransactionRequest request)
        {
            var result = await _transactionService.ProcessPurchaseTransaction(id, request);
            return Ok(result);
        }
    }
}
