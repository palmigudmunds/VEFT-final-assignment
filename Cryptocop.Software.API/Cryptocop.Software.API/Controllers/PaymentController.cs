using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Cryptocop.Software.API.Services.Interfaces;
using Cryptocop.Software.API.Models.InputModels;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/payments")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetStoredPaymentCards()
        {
            return Ok(_paymentService.GetStoredPaymentCards(User.Identity.Name));
        }

        [HttpPost]
        [Route("")]
        public IActionResult AddPaymentCard([FromBody] PaymentCardInputModel paymentCard)
        {
            _paymentService.AddPaymentCard(User.Identity.Name, paymentCard);
            return Ok(200);
        }
    }
}