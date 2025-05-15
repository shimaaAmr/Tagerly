namespace Tagerly.ViewModels.AdminViewModel
{
    public class DashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalProducts { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }

        public decimal SiteProfit => TotalRevenue * 0.10M;


        // New Property for Top Selling Products
        public List<TopProductViewModel> TopSellingProducts { get; set; }
    }
    public class TopProductViewModel
    {
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public int TotalQuantitySold { get; set; }
    }
}
