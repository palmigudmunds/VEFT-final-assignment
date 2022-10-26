using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Cryptocop.Software.API.Services.Interfaces;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/exchanges")]
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        private readonly IExchangeService _exchangeService;

        public ExchangeController(IExchangeService exchangeService)
        {
            _exchangeService = exchangeService;
        }

        [HttpGet]
        [Route("{pageNumber:int?}")]
        public async Task<IActionResult> GetExchanges(int pageNumber = 1)
        {
            var response = await _exchangeService.GetExchanges(pageNumber);

            return Ok(response);
        }
    }
}