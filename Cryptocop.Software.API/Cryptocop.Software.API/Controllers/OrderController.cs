using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetOrders()
        {
            return Ok(_orderService.GetOrders(User.Identity.Name));
        }

        [HttpPost]
        [Route("")]
        public IActionResult CreateNewOrder([FromBody] OrderInputModel order)
        {
            _orderService.CreateNewOrder(User.Identity.Name, order);
            return Ok(201);
        }
    }
}