namespace SecuritySQLite.Controllers
{
    public partial class AccountController
    {
        public class ChangePasswordRequest
        {
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
        }
    }
}
