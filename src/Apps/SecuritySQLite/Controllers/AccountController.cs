using Microsoft.AspNetCore.Mvc;
using Security.Objects.DTOs;
using Security.Services.Services.Orchestration.Interfaces;
using System.Threading.Tasks;

namespace SecuritySQLite.Controllers
{
    [Route("/Api/Account")]
    public class AccountController : Controller
    {
        private readonly IAuthenticationOrchestrationService authenticationService;

        public AccountController(IAuthenticationOrchestrationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [HttpPost("Login")]
        public async ValueTask<IActionResult> Login([FromBody] Auth auth)
            => ModelState.IsValid
                ? Ok(await authenticationService.LoginAsync(auth.User, auth.Pass))
                : BadRequest(ModelState);

        [HttpPost("Logout")]
        public async ValueTask<IActionResult> Logout()
        {
            await authenticationService.Logout();
            return Ok();
        }

        [HttpPost("ForgotPassword")]
        public async ValueTask<IActionResult> ChangePassword(string userId)
        {
            if (ModelState.IsValid)
            {
                await authenticationService.GenerateForgotPasswordToken(userId);
                return Ok();
            }
            else
                return BadRequest(ModelState);
        }
    }
}
