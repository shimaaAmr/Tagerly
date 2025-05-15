namespace Tagerly.ViewModels
{
    public class CartViewModel
    {
        public int CartId { get; set; }
        public string UserId { get; set; }
        public List<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();

        public decimal SubTotal => CartItems.Sum(i => i.Quantity * i.ProductPrice);
        public decimal SellerFee => SubTotal * 0.05m; // 5% خصم من البائع
        public decimal SellerNet => SubTotal - SellerFee; //يوصل للبائع

        // خاصية جديدة لحساب عدد العناصر الإجمالي
        public int TotalItems => CartItems.Sum(i => i.Quantity);
    }
}
