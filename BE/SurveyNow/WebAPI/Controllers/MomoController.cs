using Application.DTOs.Request.Momo;
using Microsoft.AspNetCore.Mvc;

namespace SurveyNow.Controllers
{
    [Route("api/v1/momo")]
    [ApiController]
    public class MomoController : ControllerBase
    {
        private readonly ILogger<MomoController> _logger;

        public MomoController(ILogger<MomoController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("ipn")]
        public async Task<ActionResult> OnReceivingMomoIpn([FromBody] MomoCreatePaymentResultRequest payload)
        {
            _logger.LogInformation("Receive momo ipn for transaction result".ToUpper());
            _logger.LogInformation("Transction result:\n" + payload.ToString());
            return NoContent();
        }
    }
}
