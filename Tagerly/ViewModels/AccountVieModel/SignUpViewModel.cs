
using System.ComponentModel.DataAnnotations;

namespace Tagerly.ViewModels.AccountVieModel
{
	public class SignUpViewModel
	{
		[Required]
		public string UserName { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		public string Phone { get; set; }
		public string Address { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Required]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Passwords do not match")]
		public string ConfirmPassword { get; set; }
		[Required]
		public string Role { get; set; }
	}
}
