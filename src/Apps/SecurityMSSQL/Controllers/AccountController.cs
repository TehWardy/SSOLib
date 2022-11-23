using Microsoft.AspNetCore.Mvc;
using Security.Objects.DTOs;
using Security.Services.Services.Orchestration.Interfaces;
using System.Threading.Tasks;

namespace SecuritySQLite.Controllers
{
    [Route("/Api/Account")]
    public partial class AccountController : Controller
    {
        readonly IAuthenticationOrchestrationService authenticationService;
        private readonly ISSOUserOrchestrationService userManagerService;

        public AccountController(IAuthenticationOrchestrationService authenticationService, ISSOUserOrchestrationService userManagerService)
        {
            this.authenticationService = authenticationService;
            this.userManagerService = userManagerService;
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

        [HttpPost("Register")]
        public async ValueTask<IActionResult> Register([FromBody] RegisterUser registerForm)
            => (ModelState.IsValid)
                ? Ok(await userManagerService.Register(registerForm))
                : BadRequest(ModelState);

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
