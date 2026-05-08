using ECommerce.Application.Common;
using ECommerce.Application.Common.ResultPattern;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController(IAuthService authManger) : ControllerBase
    {
        private readonly IAuthService _authManger = authManger;

        [HttpPost("register")]

        public async Task<ActionResult> Register(RegisterRequest registerRequest)
        {
            var origin = Request.Headers.Origin.ToString(); // Scheme + Host + Port (e.g., http://localhost:3000)
            var result = await _authManger.RegisterAsync(registerRequest, origin);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        [HttpPost]

        public async Task<ActionResult<Result>> Login(LoginRequest loginRequest)
        {

            var result = await _authManger.LoginAsync(loginRequest);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }


        [HttpGet("emailConfirmation")]
        public async Task<ActionResult<Result>> EmailConfirmation([FromQuery] EmailConfirmationRequest emailConfirmationRequest)
        {

            var result = await _authManger.ConfirmEmail(emailConfirmationRequest);

            return result.IsSuccess ? Ok() : result.ToProblem();

        }
    }
}














