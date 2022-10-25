using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [ApiController]
    [Route("api/cryptocurrencies")]
    public class CryptoCurrencyController : ControllerBase
    {
        private readonly ICryptoCurrencyService _cryptoCurrencyService;

        public CryptoCurrencyController(ICryptoCurrencyService cryptoCurrencyService)
        {
            _cryptoCurrencyService = cryptoCurrencyService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAvailableCryptocurrencies()
        {
            return Ok(_cryptoCurrencyService.GetAvailableCryptocurrencies());
        }
    }
}
