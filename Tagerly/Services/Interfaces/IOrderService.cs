using Tagerly.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Tagerly.Models.Enums;

namespace Tagerly.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<Order>> GetUserOrders(string userId);
        Task<Order> PlaceOrder(string userId, PaymentMethod paymentMethod);
    }
}