using System.ComponentModel.DataAnnotations;

namespace Tagerly.Models
{
	public class ResetPasswordViewModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		public string Token { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 6)]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}
}