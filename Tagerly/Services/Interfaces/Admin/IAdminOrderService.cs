using Tagerly.ViewModels.AdminViewModel;

namespace Tagerly.Services.Interfaces.Admin
{
    public interface IAdminOrderService
    {
        Task<List<AdminOrderViewModel>> GetPendingOrdersAsync();
        Task<bool> MarkAsDeliveredAsync(int orderId);
    }
}
