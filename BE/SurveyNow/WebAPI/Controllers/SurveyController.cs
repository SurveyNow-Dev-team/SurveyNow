using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Application.DTOs.Request.Survey;
using Application.DTOs.Response;
using Application.DTOs.Response.Survey;
using Application.Interfaces.Services;
using Domain.Enums;
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
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SurveyDetailResponse>> GetByIdAsync(long id)
    {
        return Ok(await _surveyService.GetByIdAsync(id));
    }

    [HttpGet]
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
}