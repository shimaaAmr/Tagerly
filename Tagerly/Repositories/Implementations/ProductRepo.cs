using Tagerly.DataAccess;
using Tagerly.Models;
using Tagerly.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tagerly.Repositories.Implementations
{
    public class ProductRepo : GenericRepo<Product>, IProductRepo
    {
        public ProductRepo(TagerlyDbContext context) : base(context) { }

        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<List<Product>> GetAllBySellerRoleAsync()
        {
            var sellerRoleId = await _context.Roles
                .Where(r => r.Name == "Seller")
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            return await _context.Products
                .Include(p => p.Seller)
                .Where(p => _context.UserRoles
                    .Any(ur => ur.UserId == p.SellerId && ur.RoleId == sellerRoleId))
                .ToListAsync();
        }




    }
}