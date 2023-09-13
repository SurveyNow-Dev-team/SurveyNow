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

        public PointController(IPointService pointService)
        {
            _pointService = pointService;
        }

        #region Purchase

        [HttpGet("purchase/history/{id}")]
        public async Task<ActionResult<PointPurchaseDetailResponse>> GetPointPurchaseDetail(long id)
        {
            var pointDetail = _pointService.GetPointPurchaseDetail(id);
            if(pointDetail == null)
            {
                throw new NotFoundException();
            }
            return Ok(pointDetail);
        }
        #endregion
        #region Redeem
        #endregion

    }
}
