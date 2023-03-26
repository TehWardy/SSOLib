using Microsoft.AspNetCore.Mvc;
using Security.Api.Interfaces;
using Security.Objects.DTOs;
using System.Threading.Tasks;

namespace Security.Api.Controllers
{
    [Route("Api/Account")]
    public class RegisterController : Controller
	{
        private readonly IAccountManager accountManager;

        public RegisterController(IAccountManager accountManager)
        {
            this.accountManager = accountManager;
        }

        [HttpPost("Register")]
        public async ValueTask<IActionResult> Register([FromBody] RegisterUser registerForm) => 
            ModelState.IsValid
                ? Ok(await accountManager.RegisterAsync(registerForm))
                : BadRequest(ModelState);

        [HttpPost("ConfirmRegistration")]
        public async ValueTask<IActionResult> ConfirmRegistration(string confirmationToken)
        {
            await accountManager.ConfirmRegistrationAsync(confirmationToken);
            return Ok();
        }
    }
}