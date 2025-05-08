using System.ComponentModel.DataAnnotations;

namespace Tagerly.ViewModels.AccountVieModel
{
	public class LoginViewModel
	{
		[EmailAddress]
		public string Email { get; set; }
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Display(Name = "Remember")]
		public bool RemmemberMe { get; set; }
	}
}
