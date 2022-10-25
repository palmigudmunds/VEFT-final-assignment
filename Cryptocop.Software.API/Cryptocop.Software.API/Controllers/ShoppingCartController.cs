using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Cryptocop.Software.API.Services.Interfaces;
using Cryptocop.Software.API.Models.InputModels;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/cart")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetCartItems()
        {
            return Ok(_shoppingCartService.GetCartItems(User.Identity.Name));
        }

        [HttpPost]
        [Route("")]
        public IActionResult AddCartItem([FromBody] ShoppingCartItemInputModel shoppingCartItem)
        {
            _shoppingCartService.AddCartItem(User.Identity.Name, shoppingCartItem);
            return Ok(201);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult RemoveCartItem(int id)
        {
            _shoppingCartService.RemoveCartItem(User.Identity.Name, id);
            return Ok();
        }

        [HttpPatch]
        [Route("{id:int}")]
        public IActionResult UpdateCartItemQuantity(int id, float quantity)
        {
            _shoppingCartService.UpdateCartItemQuantity(User.Identity.Name, id, quantity);
            return Ok();
        }

        [HttpDelete]
        [Route("")]
        public IActionResult ClearCart()
        {
            _shoppingCartService.ClearCart(User.Identity.Name);
            return Ok();
        }
    }
}