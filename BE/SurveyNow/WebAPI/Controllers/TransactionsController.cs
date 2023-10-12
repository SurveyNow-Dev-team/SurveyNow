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
            var result = await _transactionService.GetPaginatedPendingTransactionsAsync(pagingRequest);
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
            var result = await _transactionService.GetPendingTransactionsAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Hủy giao dịch đổi điểm đang chờ xử lý của người dùng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("cancel/{id:long}")]
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
        [Route("process/{id:long}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProccessRedeemTransactionResult>> ProcessRedeemTransaction([FromRoute] long id, [FromBody] UpdatePointRedeemTransactionRequest request)
        {
            var result = await _transactionService.ProcessRedeemTransaction(id, request);
            return Ok(result);
        }
    }
}
