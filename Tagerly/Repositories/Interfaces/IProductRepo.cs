using Tagerly.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tagerly.Repositories.Interfaces
{
    public interface IProductRepo : IGenericRepo<Product>
    {
<<<<<<< Updated upstream
        // يمكن إضافة دوال خاصة بالمنتجات هنا
        Task<List<Product>> GetProductsByCategoryAsync(int categoryId);
        
=======
        Task<Product> GetByIdWithCategoryAsync(int id);
        Task<IEnumerable<Product>> FindAsync(Expression<Func<Product, bool>> predicate);
        Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedAsync(
            int pageIndex,
            int pageSize,
            Expression<Func<Product, bool>> filter = null,
            Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy = null);
        Task UpdateAsync(Product product);
        Task SaveChangesAsync(); 

        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
>>>>>>> Stashed changes
    }
}