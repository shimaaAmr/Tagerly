using Tagerly.Models.Enums;

namespace Tagerly.ViewModels.AdminViewModel
{
    public class OrderDetailsViewModel
    {
        public int OrderId { get; set; }
        public string UserEmail { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }

        public string Address { get; set; }
        public string Governorate { get; set; }
        public string Notes { get; set; }

        public List<OrderItemViewModel> Items { get; set; }
    }
}
