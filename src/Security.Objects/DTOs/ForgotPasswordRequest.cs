using System;
namespace Security.Objects.DTOs
{
	public class ForgotPasswordRequest
	{
		public string Password { get; set; }
		public string Token { get; set; }
	}
}

