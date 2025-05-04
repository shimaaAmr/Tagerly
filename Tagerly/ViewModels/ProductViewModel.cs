namespace Tagerly.ViewModels
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public IFormFile ImageFile { get; set; }
        //public object SellerName { get; internal set; }

        // Seller properties
        public string SellerId { get; set; }
        public string SellerName { get; set; }
    }
}
