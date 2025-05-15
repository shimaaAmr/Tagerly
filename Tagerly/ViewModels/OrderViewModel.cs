using System.ComponentModel.DataAnnotations;
using Tagerly.Models.Enums;

namespace Tagerly.ViewModels
{
    public class OrderViewModel
    {
        [Required(ErrorMessage = "Governorate is required")]
        public Governorate SelectedGovernorate { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Payment method is required")]
        public PaymentMethod PaymentMethod { get; set; }

        [Required, EmailAddress(ErrorMessage = "Valid email is required")]
        public string Email { get; set; }

        public string Notes { get; set; }

        public bool Agreement { get; set; }
        public decimal Subtotal { get; set; }
    }
}