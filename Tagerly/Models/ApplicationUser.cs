using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
namespace Tagerly.Models
{

	public class ApplicationUser : IdentityUser
	{
		public string Address { get; set; }
		[Display(Name = "Profile Picture")]
		public string? ProfilePicturePath { get; set; }
		[ForeignKey("Cart")]
		public int? CartId { get; set; }
		[ForeignKey("Profile")]
		public int? ProfileId { get; set; }
		public Profile Profile { get; set; }
		public Cart Cart { get; set; }
		public bool IsActive { get; set; }
		public ICollection<Product> Products { get; set; }
		public ICollection<Order> Orders { get; set; }
		public ICollection<Favourite> Favourites { get; set; }
		public ICollection<Payment> Payments { get; set; }
		public ICollection<Notification> Notifications { get; set; }
	}
}
