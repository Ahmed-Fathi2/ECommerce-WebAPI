using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ECommerce.BLL.Abstractions;
using ECommerce.BLL.Managers.Auth;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IAuthManager authManager;

        public RoleController(IAuthManager authManager)
        {
            this.authManager = authManager;
        }

        [HttpPost]
        public async Task<ActionResult> AddRole(string roleName)
        {
            var result = await authManager.AddRole(roleName);
            return result.IsSuccess? Ok():result.ToProblem();
        }
    }
}



