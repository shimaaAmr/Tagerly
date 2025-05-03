namespace Tagerly.ViewModels
{
    public class CartItemViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ImageUrl { get; set; }  // تمت إضافة هذا الحقل
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }

        // خاصية محسوبة للسعر الإجمالي للعنصر
        public decimal TotalPrice => Quantity * ProductPrice;
    }
}
