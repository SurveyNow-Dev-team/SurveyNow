using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SuerveyNow.Controllers
{
    [Route("api/address")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressesController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        // GET api/<AddressesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AddressResponse>> Get(long id)
        {
            var address = await _addressService.GetAddress(id);
            if (address == null)
            {
                return NotFound();
            }
            return Ok(address);
        }

        // POST api/<AddressesController>
        [HttpPost]
        public async void Post(AddressRequest request)
        {
            await _addressService.CreateAddress(request);
        }

        // PUT api/<AddressesController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<AddressResponse>> Put(int id, AddressRequest request)
        {
            var address = await _addressService.UpdateAddress(id, request);
            if (address == null)
            {
                return NotFound();
            }
            return Ok(address);
        }

        // DELETE api/<AddressesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
