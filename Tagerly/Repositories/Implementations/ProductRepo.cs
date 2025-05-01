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

       
    
        
    }
}