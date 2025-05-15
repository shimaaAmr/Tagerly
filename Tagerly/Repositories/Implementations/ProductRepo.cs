using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tagerly.DataAccess.DbContexts;
using Tagerly.Models;
using Tagerly.Models.Enums;
using Tagerly.Repositories.Interfaces;
using Tagerly.ViewModels.AdminViewModel;

namespace Tagerly.Repositories.Implementations
{
    public class ProductRepo : GenericRepo<Product>, IProductRepo
    {
        #region Fields & Constructor
        private readonly TagerlyDbContext _context;
        private readonly DbSet<Product> _dbSet;

        public ProductRepo(TagerlyDbContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = context.Set<Product>();
        }
        #endregion

        #region Extended Read Operations
        public async Task<Product> GetByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.Seller)
                .FirstOrDefaultAsync(p => p.Id == id && p.Status != ProductStatus.Deleted);
        }

        public async Task<Product> GetByIdWithCategoryAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id && p.Status != ProductStatus.Deleted);
        }

        public async Task<IEnumerable<Product>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.Seller)
                .Where(p => p.Status != ProductStatus.Deleted)
                .ToListAsync();
        }

        public new async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _dbSet
                .Where(p => p.Status != ProductStatus.Deleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId && p.Status != ProductStatus.Deleted)
                .ToListAsync();
        }

        public async Task<List<Product>> GetAllBySellerRoleAsync()
        {
            var sellerRoleId = await _context.Roles
                .Where(r => r.Name == "Seller")
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            return await _dbSet
                .Include(p => p.Seller)
                .Where(p => _context.UserRoles
                    .Any(ur => ur.UserId == p.SellerId && ur.RoleId == sellerRoleId) &&
                    p.Status != ProductStatus.Deleted)
                .ToListAsync();
        }
        #endregion

        #region Filtered/Search Operations
        public new async Task<IEnumerable<Product>> FindAsync(Expression<Func<Product, bool>> predicate)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Where(p => p.Status != ProductStatus.Deleted)
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedAsync(
            int pageIndex,
            int pageSize,
            Expression<Func<Product, bool>> filter = null,
            Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy = null)
        {
            IQueryable<Product> query = _dbSet
                .Include(p => p.Category)
                .Where(p => p.Status != ProductStatus.Deleted);

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var totalCount = await query.CountAsync();

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
        #endregion

        #region Status Management
        public async Task<bool> SoftDeleteAsync(int id)
        {
            var product = await _dbSet.FindAsync(id);
            if (product == null || product.Status == ProductStatus.Deleted)
                return false;

            product.Status = ProductStatus.Deleted;
            _dbSet.Update(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApproveProductAsync(int id, bool isApproved)
        {
            var product = await _dbSet.FindAsync(id);
            if (product == null || product.Status == ProductStatus.Deleted)
                return false;

            product.IsApproved = isApproved;
            _dbSet.Update(product);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Save Operations
        public new async Task UpdateAsync(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            if (product.Status == ProductStatus.Deleted)
                throw new InvalidOperationException("Cannot update a deleted product");

            _dbSet.Update(product);
            await Task.CompletedTask;
        }

        public new async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Analytics
        public async Task<List<TopProductViewModel>> GetTopSellingProductsAsync(int count)
        {
            return await _context.OrderDetails
                .GroupBy(od => new { od.Product.Id, od.Product.Name, od.Product.ImageUrl })
                .Select(g => new TopProductViewModel
                {
                    ProductName = g.Key.Name,
                    ImageUrl = g.Key.ImageUrl,
                    TotalQuantitySold = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(p => p.TotalQuantitySold)
                .Take(count)
                .ToListAsync();
        }
        #endregion
    }
}