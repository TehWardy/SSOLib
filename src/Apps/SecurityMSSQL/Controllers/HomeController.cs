using Microsoft.AspNetCore.Mvc;
using Security.Objects;

namespace SecurityMSSQL.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ISSOAuthInfo authInfo;

        public HomeController(ISSOAuthInfo authInfo) =>
            this.authInfo = authInfo;

        [HttpGet]
        public IActionResult Get() =>
            Ok(authInfo.SSOUserId);
    }
}
