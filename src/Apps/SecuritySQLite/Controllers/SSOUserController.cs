using System;
using Microsoft.AspNetCore.Mvc;
using Security.Services.Services.Processing.Interfaces;

namespace SecuritySQLite.Controllers
{
    [Route("Api/Security/SSOUser")]
	public class SSOUserController : Controller
	{
        private readonly ISSOUserProcessingService ssoUserProcessingService;

        public SSOUserController(ISSOUserProcessingService ssoUserProcessingService)
		{
            this.ssoUserProcessingService = ssoUserProcessingService;
        }

        [HttpGet("Me()")]
        public IActionResult Me()
            => Ok(ssoUserProcessingService.Me());
	}
}

