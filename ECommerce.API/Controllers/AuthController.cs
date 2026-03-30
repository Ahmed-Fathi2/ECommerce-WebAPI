using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ECommerce.BLL.Abstractions;
using ECommerce.BLL.Dtos.Auth;
using ECommerce.BLL.Managers.Auth;
using ECommerce.BLL.Abstractions.ResultPattern;

namespace ECommerce.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManager _authManger;

        public AuthController(IAuthManager authManger)
        {
            _authManger = authManger;
        }
        [HttpPost("register")]

        public async Task<ActionResult> Register(RegisterRequest registerRequest)
        {
            var origin = Request.Headers.Origin.ToString(); // Scheme + Host + Port (e.g., http://localhost:3000)
            var result = await _authManger.RegisterAsync(registerRequest,origin);  
            
            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        [HttpPost]

        public async Task<ActionResult<Result>> Login(LoginRequest loginRequest)
        {

            var result = await _authManger.LoginAsync(loginRequest);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }


        [HttpGet("emailConfirmation")]
        public async Task<ActionResult<Result>> EmailConfirmation([FromQuery]EmailConfirmationRequest emailConfirmationRequest)
        {

            var result = await _authManger.ConfirmEmail(emailConfirmationRequest);

            return result.IsSuccess ? Ok() : result.ToProblem();
          
        }
    }
}







