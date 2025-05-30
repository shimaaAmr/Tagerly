﻿using Tagerly.ViewModels.AdminViewModel;

namespace Tagerly.Services.Interfaces.Admin
{
    public interface IAdminOrderService
    {
        Task<List<AdminOrderViewModel>> GetPendingOrdersAsync();
        Task<bool> MarkAsDeliveredAsync(int orderId);
        Task<List<AdminOrderViewModel>> GetFilteredOrdersAsync(string status, string search);

        Task<OrderDetailsViewModel> GetOrderDetailsViewModelByIdAsync(int orderId);




    }
}
