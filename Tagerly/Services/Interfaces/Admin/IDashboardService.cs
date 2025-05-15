using Tagerly.ViewModels.AdminViewModel;

namespace Tagerly.Services.Interfaces.Admin
{
    public interface IDashboardService
    {
        Task<int> GetTotalUsersAsync();
        Task<int> GetTotalProductsAsync();
        Task<int> GetTotalOrdersAsync();
        Task<decimal> GetTotalRevenueAsync();
        Task<List<TopProductViewModel>> GetTopSellingProductsAsync(int count);


    }
}
