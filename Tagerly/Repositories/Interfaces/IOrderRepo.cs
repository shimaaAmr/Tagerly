using Tagerly.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Tagerly.Repositories.Interfaces
{
    public interface IOrderRepo : IBaseRepo<Order>
    {
        Task<List<Order>> GetUserOrdersAsync(string userId);
        Task<Order> CreateOrderFromCartAsync(string userId, Payment payment);
    }
}