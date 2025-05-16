using Microsoft.AspNetCore.Http;

namespace Tagerly.ViewModels
{
    public class ProductViewModel
    {
        #region Product Properties
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        #endregion

        #region Image Handling
        public string ImageUrl { get; set; }
        public IFormFile ImageFile { get; set; }
        #endregion

        #region Category Information
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        #endregion

        #region Seller Information
        public string SellerId { get; set; }
        public string SellerName { get; set; }
        #endregion

        #region Approval Status
        public bool? IsApproved { get; set; } 
        #endregion
    }
}