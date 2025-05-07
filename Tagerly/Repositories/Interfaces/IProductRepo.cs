using Tagerly.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;

using System.Linq.Expressions;
using Tagerly.Models;


namespace Tagerly.Repositories.Interfaces
{
	public interface IProductRepo : IGenericRepo<Product>
	{

		// يمكن إضافة دوال خاصة بالمنتجات هنا

		//Task<List<Product>> GetProductsByCategoryAsync(int categoryId);
		Task<List<Product>> GetAllBySellerRoleAsync();
		Task<Product> GetByIdWithCategoryAsync(int id);
		Task<IEnumerable<Product>> FindAsync(Expression<Func<Product, bool>> predicate);
		Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedAsync(
			int pageIndex,
			int pageSize,
			Expression<Func<Product, bool>> filter = null,
			Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy = null);
		Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);

		Task UpdateAsync(Product product);
		Task SaveChangesAsync();

		Task<bool> SoftDeleteAsync(int id);
		Task<IEnumerable<Product>> GetAllWithDetailsAsync();
		Task<IEnumerable<Product>> GetAllAsync();

        Task<bool> ApproveProductAsync(int id, bool isApproved);
        Task<Product>GetByIdWithDetailsAsync(int id);


    }
}