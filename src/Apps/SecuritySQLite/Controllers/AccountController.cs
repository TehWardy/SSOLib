using Microsoft.AspNetCore.Mvc;
using Security.Objects.DTOs;
using Security.Services.Services.Orchestration.Interfaces;
using System.Threading.Tasks;

namespace SecuritySQLite.Controllers
{
    [Route("/Api/Account")]
    public class AccountController : Controller
    {
        readonly ISSOUserAuthenticationOrchestrationService userService;

        public AccountController(ISSOUserAuthenticationOrchestrationService userService)
            => this.userService = userService;

        public class Auth
        { 
            public string User { get; set; }
            public string Pass { get; set; }
        }

        public class ChangePasswordRequest
        {
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
        }

        [HttpPost("Login")]
        public async ValueTask<IActionResult> Login([FromBody] Auth auth)
            => ModelState.IsValid
                ? Ok(await userService.Login(auth.User, auth.Pass))
                : BadRequest(ModelState);

        [HttpPost("Logout")]
        public async ValueTask<IActionResult> Logout()
        {
            await userService.Logout();
            return Ok();
        }

        [HttpPost("Register")]
        public async ValueTask<IActionResult> Register([FromBody] RegisterUser registerForm)
            => (ModelState.IsValid)
                ? Ok(await userService.Register(registerForm))
                : BadRequest(ModelState);

        [HttpPost("ChangePassword")]
        public async ValueTask<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            if (ModelState.IsValid)
            {
                await userService.ChangePassword(request.OldPassword, request.NewPassword);
                return Ok();
            }
            else 
                return BadRequest(ModelState);
        }
    }
}
