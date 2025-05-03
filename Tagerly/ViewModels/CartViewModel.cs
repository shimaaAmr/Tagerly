namespace Tagerly.ViewModels
{
    public class CartViewModel
    {
        public int CartId { get; set; }
        public string UserId { get; set; }
        public List<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();

        public decimal SubTotal => CartItems.Sum(i => i.Quantity * i.ProductPrice);
        public decimal Total => SubTotal * 1.1m; // Including 10% tax

        // خاصية جديدة لحساب عدد العناصر الإجمالي
        public int TotalItems => CartItems.Sum(i => i.Quantity);
    }
}
