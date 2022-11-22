using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Security.Objects.DTOs;
using Security.Services.Services.Orchestration.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security;
using System.Threading.Tasks;

namespace SecuritySQLite.Controllers
{
    [Route("/Api/Account")]
    public class AccountController : RESTFulController
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
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                return Ok(await authenticationService.LoginAsync(auth.User, auth.Pass));
            }
            catch(ValidationException ex)
            {
                return BadRequest(ex);
            }
            catch (SecurityException ex)
            {
                return Unauthorized(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost("Logout")]
        public async ValueTask<IActionResult> Logout()
        {
            await authenticationService.Logout();
            return Ok();
        }

        [HttpPost("Register")]
        public async ValueTask<IActionResult> Register([FromBody] RegisterUser registerForm)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                return Ok(await userManagerService.Register(registerForm));
            }
            catch(ValidationException ex)
            {
                return BadRequest(ex);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost("ChangePassword")]
        public async ValueTask<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            await userManagerService.ChangePassword(request.OldPassword, request.NewPassword);
            return Ok();
        }
    }
}
