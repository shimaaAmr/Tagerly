using System.ComponentModel.DataAnnotations;
using Tagerly.Models.Enums;

namespace Tagerly.ViewModels
{
    public class OrderViewModel
    {
        [Required(ErrorMessage = "Shipping address is required")]
        public string ShippingAddress { get; set; }

        [Required(ErrorMessage = "Billing address is required")]
        public string BillingAddress { get; set; }

        [Required(ErrorMessage = "Payment method is required")]
        public PaymentMethod PaymentMethod { get; set; }

        [Required, EmailAddress(ErrorMessage = "Valid email is required")]
        public string Email { get; set; }

        public string Notes { get; set; }

        [Display(Name = "I agree to terms & conditions")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the terms")]
        public bool Agreement { get; set; }
        public decimal Subtotal { get; set; }
    }
}