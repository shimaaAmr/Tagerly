using Tagerly.Models;
using Tagerly.Models.Enums;
using Tagerly.ViewModels.AdminViewModel;

namespace Tagerly.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<Order> PlaceOrder(
            string userId,
            PaymentMethod paymentMethod,
            Governorate SelectedGovernorate ,
            string Address, // أضف هذا الباراميتر
            string email,
            string notes); // أضف هذا الباراميتر


       
    }
}