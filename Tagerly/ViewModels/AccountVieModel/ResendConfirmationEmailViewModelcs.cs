using System.ComponentModel.DataAnnotations;

namespace Tagerly.Models
{
	public class ResendConfirmationEmailViewModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}