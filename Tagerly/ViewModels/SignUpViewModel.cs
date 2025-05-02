using System.ComponentModel.DataAnnotations;

namespace Tagerly.ViewModels
{
	public class SignUpViewModel
	{
		[Display(Name = "Name")]
		public string UserName { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Address { get; set; }
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
		public string Role { get; set; }
	}
}
