namespace Tagerly.ViewModels
{
    public class OrderConfirmationViewModel
    {
        public int OrderId { get; set; }
        public string TransactionId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string? ShippingAddress { get; set; }
        public List<OrderItemViewModel> Items { get; set; }
    }
}
