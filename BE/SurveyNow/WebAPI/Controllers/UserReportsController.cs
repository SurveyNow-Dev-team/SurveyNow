﻿using Application.DTOs.Request.User;
using Application.DTOs.Response.User;
using Application.Interfaces.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SurveyNow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserReportsController : ControllerBase
    {
        private readonly IUserReportService _userReportService;

        public UserReportsController(IUserReportService userReportService)
        {
            _userReportService = userReportService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReportResponse>>> GetAll([FromQuery] UserReportFilter? filter)
        {
            return Ok(await _userReportService.Get(filter));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserReportResponse>> Get([FromRoute] long id)
        {
            return Ok(await _userReportService.Get(id));
        }

        /// <summary>
        /// Change user report status, admin role only
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request">status: Approved or Rejected</param>
        /// <returns></returns>
        [HttpPut("status/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserReportResponse>> ChangeReportStatus([FromRoute] long id, [FromBody] UserReportStatusRequest request)
        {
            return Ok(await _userReportService.ChangeReportStatus(id, request));
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserReportRequest request)
        {
            await _userReportService.Create(request);
            return Ok();
        }
    }
}
