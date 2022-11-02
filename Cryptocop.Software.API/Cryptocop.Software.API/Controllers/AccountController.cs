using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {   
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;

        public AccountController(IAccountService accountService, ITokenService tokenService)
        {
            _accountService = accountService;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public IActionResult CreateUser([FromBody] RegisterInputModel register)
        {
            return Ok(_accountService.CreateUser(register));
        }
        
        [AllowAnonymous]
        [HttpPost]
        [Route("signin")]
        public IActionResult AuthenticateUser([FromBody] LoginInputModel login)
        {
            // Call a authentication service
            var user = _accountService.AuthenticateUser(login);
            if (user == null) { return Unauthorized(); }
            
            // Return a valid JWT token
            var token = _tokenService.GenerateJwtToken(user);
            return Ok(token);
        }

        [HttpGet]
        [Route("signout")]
        public IActionResult Logout()
        {
            // Retrieve token id from claim and blacklist token
            int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "tokenId").Value, out var tokenId);
            _accountService.Logout(tokenId);
            return NoContent();
        }
    }
}