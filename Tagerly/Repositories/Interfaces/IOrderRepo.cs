// في IOrderRepo.cs
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using Tagerly.Models;

namespace Tagerly.Repositories.Interfaces
{
    public interface IOrderRepo : IGenericRepo<Order>
    {
        Task<List<Order>> GetUserOrdersAsync(string userId);
        Task<Order> GetOrderByIdWithDetails(int orderId);

        // إضافة دعم للمعاملات
        Task<IDbContextTransaction> BeginTransactionAsync();

        // التحكم فى order
        Task<IEnumerable<Order>> FindAsync(Expression<Func<Order, bool>> predicate);
        Task<Order> GetByIdAsync(int id);
        Task UpdateAsync(Order order);
        IQueryable<Order> GetAllWithUserAndDetails();

        Task SaveChangesAsync();
    }
}