using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SurveyNow.Controllers
{
    [Route("api/hobby")]
    [ApiController]
    public class HobbiesController : ControllerBase
    {
        private readonly IHobbyService _hobbyService;

        public HobbiesController(IHobbyService hobbyService)
        {
            _hobbyService = hobbyService;
        }


        // GET api/<HobbiesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HobbyResponse>> Get(int id)
        {
            var hobby = await _hobbyService.GetHobby(id);
            if(hobby == null)
            {
                return NotFound();
            }
            return Ok(hobby);
        }

        // POST api/<HobbiesController>
        [HttpPost]
        public async Task Post(HobbyRequest request)
        {
            await _hobbyService.CreateHobby(request);
        }

        // PUT api/<HobbiesController>/5
        [HttpPut("{id}")]
        public async Task Put(long id, [FromBody] HobbyRequest request)
        {
            await _hobbyService.UpdateHobby(id, request);
        }
    }
}
