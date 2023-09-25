using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Application.DTOs.Request.Survey;
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
    public async Task<SurveyDetailResponse> GetByIdAsync(long id)
    {
        return await _surveyService.GetByIdAsync(id);
    }
    
}