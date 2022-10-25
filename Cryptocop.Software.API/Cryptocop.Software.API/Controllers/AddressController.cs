using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/addresses")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAllAddresses()
        {
            return Ok(_addressService.GetAllAddresses(User.Identity.Name));
        }

        [HttpPost]
        [Route("")]
        public IActionResult AddAddress([FromBody] AddressInputModel address)
        {
            _addressService.AddAddress(User.Identity.Name, address);
            return Ok(200);
        }

        [HttpDelete]
        [Route("{addressId:int}")]
        public IActionResult DeleteAddress(int addressId)
        {
            _addressService.DeleteAddress(User.Identity.Name, addressId);
            return Ok();
        }
    }
}