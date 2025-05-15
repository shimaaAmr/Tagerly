using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Tagerly.Models.Enums;

namespace Tagerly.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }
        public string Status { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("Payment")]
        public int PaymentId { get; set; }
        public ApplicationUser User { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
        public Payment Payment { get; set; } =new Payment();
        public Governorate SelectedGovernorate { get; set; }

        public string  Address { get; set; }
        public string PaymentMethod { get; set; }
        public string Notes { get; set; }
        public string Email { get; set; }
        public decimal TotalAmount { get; set; }
      
    }
}