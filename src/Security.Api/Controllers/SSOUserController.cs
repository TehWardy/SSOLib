using Microsoft.AspNetCore.Mvc;
using Security.Api.Interfaces;

namespace Security.Api.Controllers
{
    [Route("Api/Security/SSOUser")]
	public class SSOUserController : Controller
	{
        private readonly IAccountManager accountManager;

        public SSOUserController(IAccountManager ssoUserProcessingService) =>
            this.accountManager = ssoUserProcessingService;

        [HttpGet("Me()")]
        public IActionResult Me() => 
            Ok(accountManager.Me());

        /*
        [HttpGet]
        [EnableQuery]
        public IActionResult GetAllSSOUsers()
            => Ok(accountManager.GetAllUsers(ignoreFilters: false));
        */
	}
}