using System.ComponentModel.DataAnnotations.Schema;

namespace Tagerly.Models
{
    public class Cart
    {
        public int Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public ICollection<CartItem> CartItems { get; set; }
    }
}