using System.ComponentModel.DataAnnotations;
using Tagerly.Models.Enums;

namespace Tagerly.ViewModels
{
    public class CreateOrderViewModel
    {
        [Required(ErrorMessage = "Shipping address is required")]
        public string ShippingAddress { get; set; }

        public string BillingAddress { get; set; }

        [Required(ErrorMessage = "Payment method is required")]
        public PaymentMethod PaymentMethod { get; set; }
        public string Email { get; set; }
        public string Notes { get; set; }
    }
}