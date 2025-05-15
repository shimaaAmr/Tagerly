using Microsoft.EntityFrameworkCore;
using Tagerly.DataAccess.DbContexts;
using Tagerly.Services.Interfaces.Admin;
using Tagerly.ViewModels.AdminViewModel;
using Tagerly.Repositories.Interfaces;

namespace Tagerly.Services.Implementations.Admin
{
    public class DashboardService:IDashboardService
    {
        private readonly TagerlyDbContext _context;
        private readonly IProductRepo _productRepo;

        public DashboardService(TagerlyDbContext context, IProductRepo productRepo)
        {
           _context = context;
            _productRepo = productRepo;
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

        public async Task<List<TopProductViewModel>> GetTopSellingProductsAsync(int count)
        {
            return await _productRepo.GetTopSellingProductsAsync(count);
        }
    }
}
