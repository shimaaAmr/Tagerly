using System.ComponentModel.DataAnnotations;

namespace Tagerly.ViewModels
{
    public class CheckoutViewModel
    {
        // Contact Info
        [Required, EmailAddress]
        public string Email { get; set; }
        public bool SubscribeToNews { get; set; }

        // Shipping Address
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Zip { get; set; }

        public bool UseSameAddress { get; set; }

        // Billing Address (optional if different)
        public string FirstNameAddress { get; set; }
        public string LastNameAddress { get; set; }
        public string AddressAddress { get; set; }
        public string CountryAddress { get; set; }
        public string StateAddress { get; set; }
        public string ZipAddress { get; set; }

        // Shipping Method
        public string ShippingMethod { get; set; }

        // Payment Method
        public string PaymentMethod { get; set; }
    }

}
