namespace Tagerly.ViewModels
{
    public class OrderItemViewModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ImageUrl { get; set; }  // ممكن تخليه optional لو مش بتخزني صور

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice => Price * Quantity;
    }
}
