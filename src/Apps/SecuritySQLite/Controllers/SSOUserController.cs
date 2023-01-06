using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Security.Services.Services.Processing.Interfaces;

namespace SecuritySQLite.Controllers
{
	public class SSOUserController : ODataController
	{
        private readonly ISSOUserProcessingService ssoUserProcessingService;

        public SSOUserController(ISSOUserProcessingService ssoUserProcessingService)
		{
            this.ssoUserProcessingService = ssoUserProcessingService;
        }

        [HttpGet("Me()")]
        public IActionResult Me()
            => Ok(ssoUserProcessingService.Me());

        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
            => Ok(ssoUserProcessingService.GetAllSSOUsers(ignoreFilters: false));
    }
}