using Tagerly.Models.Enums;

namespace Tagerly.ViewModels.AdminViewModel
{
    public class ProductApprovingVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SellerName { get; set; }
        public string ImageUrl { get; set; }
        public bool? IsApproved { get; set; } = null;       // null = لسه، true = موافق، false = مرفوض
        public bool IsReject { get; set; } = false;        // للحذف المنطقي

        public ProductStatus Status { get; set; }
    }
}

