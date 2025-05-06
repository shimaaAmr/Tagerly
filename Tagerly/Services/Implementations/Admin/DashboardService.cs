using Microsoft.EntityFrameworkCore;
using Tagerly.DataAccess.DbContexts;
using Tagerly.Services.Interfaces.Admin;

namespace Tagerly.Services.Implementations.Admin
{
    public class DashboardService:IDashboardService
    {
        private readonly TagerlyDbContext _context;

        public DashboardService(TagerlyDbContext context)
        {
           _context = context;
        }

        public async Task<int> GetTotalUsersAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<int> GetTotalProductsAsync()
        {
            return await _context.Products.CountAsync();
        }

        public async Task<int> GetTotalOrdersAsync()
        {
            return await _context.Orders.CountAsync();
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _context.OrderDetails
                .SumAsync(od => od.Price * od.Quantity);
        }
    }
}
