using System.ComponentModel.DataAnnotations;

namespace Tagerly.Models
{
	public class ForgotPasswordViewModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}