using System.ComponentModel.DataAnnotations;

namespace Security.Objects.DTOs
{

    public class RegisterUser
    {
        public string DisplayName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Culture { get; set; }

        public string PhoneNumber { get; set; }

        public int AppId { get; set; }
    }
}