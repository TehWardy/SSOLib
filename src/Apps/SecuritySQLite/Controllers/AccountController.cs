using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RESTFulSense.Controllers;
using Security.Data.Brokers.Authentication;
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
        private readonly IIdentityBroker identityBroker;

        public AccountController(IAuthenticationOrchestrationService authenticationService, ISSOUserOrchestrationService userManagerService, IIdentityBroker identityBroker)
        {
            this.authenticationService = authenticationService;
            this.userManagerService = userManagerService;
            this.identityBroker = identityBroker;
        }

        [HttpGet("Me")]
        public IActionResult Me()
            => Ok(identityBroker.Me());

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

        [HttpPost("ForgotPassword")]
        public async ValueTask<IActionResult> ForgotPassword(string userId)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await authenticationService.GenerateForgotPasswordToken(userId);
                return Ok();
            }
            catch (ValidationException ex)
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

        [HttpPost("ConfirmForgotPassword")]
        public async ValueTask<IActionResult> ConfirmForgotPassword([FromBody] ForgotPasswordRequest forgotPasswordRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await userManagerService.ConfirmForgotPassword(forgotPasswordRequest.Token, forgotPasswordRequest.Password);
                return Ok();
            }
            catch (ValidationException ex)
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

        [HttpPost("ConfirmRegistration")]
        public async ValueTask<IActionResult> ConfirmRegistration(string confirmationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await userManagerService.ConfirmRegistration(confirmationToken);
                return Ok();
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
    }
}
