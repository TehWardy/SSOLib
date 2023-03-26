using Microsoft.AspNetCore.Mvc;
using Security.Objects.DTOs;
using Security.Services.Orchestration.Interfaces;
using System.Threading.Tasks;

namespace SecuritySQLite.Controllers
{
    [Route("/Api/Register")]
    public class RegisterController : Controller
	{
        private readonly ISSOUserOrchestrationService userManagerService;

        public RegisterController(ISSOUserOrchestrationService userManagerService)
        {
            this.userManagerService = userManagerService;
        }

        [HttpPost("Register")]
        public async ValueTask<IActionResult> Register([FromBody] RegisterUser registerForm)
            => (ModelState.IsValid)
                ? Ok(await userManagerService.Register(registerForm))
                : BadRequest(ModelState);

        [HttpPost("ConfirmRegistration")]
        public async ValueTask<IActionResult> ConfirmRegistration(string confirmationToken)
        {
            await userManagerService.ConfirmRegistration(confirmationToken);
            return Ok();
        }
    }
}

