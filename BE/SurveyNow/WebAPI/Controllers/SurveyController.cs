using Application.DTOs.Request.Survey;
using Application.DTOs.Response;
using Application.DTOs.Response.Survey;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SurveyNow.Controllers;

[Route("api/v1/surveys")]
[ApiController]
[Authorize]
public class SurveyController : ControllerBase
{
    private readonly ISurveyService _surveyService;
    private readonly ILogger<SurveyController> _logger;

    public SurveyController(ISurveyService surveyService, ILogger<SurveyController> logger)
    {
        _surveyService = surveyService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<long>> CreateSurveyAsync([FromBody] SurveyRequest request)
    {
        var result = await _surveyService.CreateSurveyAsync(request);
        return CreatedAtAction(nameof(CommonFilterAsync), new { id = result }, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SurveyDetailResponse>> GetByIdAsync(long id)
    {
        return Ok(await _surveyService.GetByIdAsync(id));
    }

    [HttpGet]
    public async Task<ActionResult<PagingResponse<CommonSurveyResponse>>> CommonFilterAsync(
        [FromQuery] string? status,
        [FromQuery] string? title,
        [FromQuery] string? sortTitle,
        [FromQuery] string? sortTotalQuestion,
        [FromQuery] string? sortPoint,
        [FromQuery] string? sortStartDate,
        [FromQuery] string? sortExpiredDate,
        [FromQuery] int? page,
        [FromQuery] int? size
    )
    {
        return Ok(await _surveyService.FilterCommonSurveyAsync(status, title, sortTitle, sortTotalQuestion, sortPoint,
            sortStartDate, sortExpiredDate, page, size));
    }

    [HttpGet("/api/v1/admin/surveys")]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<PagingResponse<SurveyResponse>>> FilterAsync(
        [FromQuery] string? status,
        [FromQuery] bool? isDelete,
        [FromQuery] string? packType,
        [FromQuery] string? title,
        [FromQuery] string? sortTitle,
        [FromQuery] string? sortCreatedDate,
        [FromQuery] string? sortStartDate,
        [FromQuery] string? sortExpiredDate,
        [FromQuery] string? sortModifiedDate,
        [FromQuery] int? page,
        [FromQuery] int? size)
    {
        return Ok(await _surveyService.FilterSurveyAsync(status, isDelete, packType, title, sortTitle, sortCreatedDate,
            sortStartDate, sortExpiredDate, sortModifiedDate, page, size));
    }

    [HttpGet("/api/v1/account/surveys")]
    public async Task<ActionResult<PagingResponse<SurveyResponse>>> FilterAccountSurveyAsync(
        [FromQuery] string? status,
        [FromQuery] string? packType,
        [FromQuery] string? title,
        [FromQuery] string? sortTitle,
        [FromQuery] string? sortCreatedDate,
        [FromQuery] string? sortStartDate,
        [FromQuery] string? sortExpiredDate,
        [FromQuery] string? sortModifiedDate,
        [FromQuery] int? page,
        [FromQuery] int? size
    )
    {
        return Ok(await _surveyService.FilterAccountSurveyAsync(status, packType, title, sortTitle, sortCreatedDate,
            sortStartDate, sortExpiredDate, sortModifiedDate, page, size));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSurvey(long id)
    {
        await _surveyService.DeleteSurveyAsync(id);
        return Ok();
    }
}