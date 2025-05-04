// في IOrderRepo.cs
using Microsoft.EntityFrameworkCore.Storage;
using Tagerly.Models;

namespace Tagerly.Repositories.Interfaces
{
    public interface IOrderRepo : IGenericRepo<Order>
    {
        Task<List<Order>> GetUserOrdersAsync(string userId);
        Task<Order> GetOrderByIdWithDetails(int orderId);
        Task SaveChangesAsync();

        // إضافة دعم للمعاملات
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}