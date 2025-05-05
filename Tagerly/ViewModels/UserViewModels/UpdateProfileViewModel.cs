using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Tagerly.Models
{
	public class UpdateProfileViewModel
	{
		public string Id { get; set; }

		[Display(Name = "Username")]
		public string UserName { get; set; }

		[Display(Name = "Email")]
		[EmailAddress]
		public string Email { get; set; }

		[Display(Name = "Address")]
		public string Address { get; set; }

		[Display(Name = "Phone")]
		public string PhoneNumber { get; set; }

		[Display(Name = "Profile Picture")]
		public IFormFile? ProfilePicture { get; set; } // For file upload

		public string? ProfilePicturePath { get; set; } // Stored path
	}
}