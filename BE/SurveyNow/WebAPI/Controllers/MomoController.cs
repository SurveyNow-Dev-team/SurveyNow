using Application.DTOs.Request.Momo;
using Application.DTOs.Request.Point;
using Application.DTOs.Response.Momo;
using Application.DTOs.Response.Point;
using Application.Interfaces.Services;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace SurveyNow.Controllers
{
    [Route("api/v1/momo")]
    [ApiController]
    public class MomoController : ControllerBase
    {
        private readonly ILogger<MomoController> _logger;
        private readonly IMomoService momoService;

        public MomoController(ILogger<MomoController> logger, IMomoService momoService)
        {
            _logger = logger;
            this.momoService = momoService;
        }

        /// <summary>
        /// Endpoint chỉ dùng để momo gửi kết quả giao dịch về.
        /// Client ko sử dụng endpoint này
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ipn")]
        public async Task<ActionResult> OnReceivingMomoIpn([FromBody] MomoCreatePaymentResultRequest payload)
        {
            _logger.LogInformation("Receive momo ipn for transaction result".ToUpper());
            _logger.LogInformation("Transction result:\n" + payload.ToString());
            return NoContent();
        }

        /// <summary>
        /// Test endpoint
        /// </summary>
        /// <param name="redeemRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("redeem")]
        public async Task<ActionResult<MomoPointRedeemResponse>> ProcessMomoPointRedeem([FromBody] PointRedeemRequest redeemRequest)
        {
            var result = await momoService.ProcessMomoRedeemGiftAsync(redeemRequest);
            return Ok(result);
        }
    }
}
