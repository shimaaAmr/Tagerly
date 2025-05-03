using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tagerly.ViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }

        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; }

        public List<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel>();

        [Display(Name = "Shipping Address")]
        [Required(ErrorMessage = "Shipping address is required")]
        public string ShippingAddress { get; set; }

        [Display(Name = "Billing Address")]
        public string BillingAddress { get; set; }
    }


    public class OrderItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => Price * Quantity;
    }
}
