using Tagerly.Models.Enums;

namespace Tagerly.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        public bool IsDeleted { get; set; } = false;        // للحذف المنطقي
        public bool? IsApproved { get; set; } = null;       // null = لسه، true = موافق، false = مرفوض
        public ProductStatus Status { get; set; }
        public string SellerId { get; set; }

        // Navigation Property
        public ApplicationUser Seller { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<Favourite> Favourites { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}