using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tagerly.Models;

namespace Tagerly.Repositories.Interfaces
{
    public interface IProductRepo : IGenericRepo<Product>
    {
        #region Extended Read Operations
        Task<Product> GetByIdWithDetailsAsync(int id);
        Task<Product> GetByIdWithCategoryAsync(int id);
        Task<IEnumerable<Product>> GetAllWithDetailsAsync();
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<List<Product>> GetAllBySellerRoleAsync();
        #endregion

        #region Filtered/Search Operations
        Task<IEnumerable<Product>> FindAsync(Expression<Func<Product, bool>> predicate);
        Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedAsync(
            int pageIndex,
            int pageSize,
            Expression<Func<Product, bool>> filter = null,
            Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy = null);
        #endregion

        #region Status Management
        Task<bool> SoftDeleteAsync(int id);
        Task<bool> ApproveProductAsync(int id, bool isApproved);
        #endregion

        #region Save Operations
        Task UpdateAsync(Product product);
        Task SaveChangesAsync();
        #endregion
    }
}