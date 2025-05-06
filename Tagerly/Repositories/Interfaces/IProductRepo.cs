﻿//using System.Linq.Expressions;
//using Tagerly.Models;

//namespace Tagerly.Repositories.Interfaces
//{
//	public interface IProductRepo : IGenericRepo<Product>
//	{

//		// يمكن إضافة دوال خاصة بالمنتجات هنا

//		Task<List<Product>> GetProductsByCategoryAsync(int categoryId);


//		Task<Product> GetByIdWithCategoryAsync(int id);

//		//Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
//		Task<List<Product>> GetAllBySellerRoleAsync();
//		//Task<Product> GetByIdWithCategoryAsync(int id);

//		Task<IEnumerable<Product>> FindAsync(Expression<Func<Product, bool>> predicate);
//		Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedAsync(
//			int pageIndex,
//			int pageSize,
//			Expression<Func<Product, bool>> filter = null,
//			Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy = null);

//		Task UpdateAsync(Product product);
//		Task SaveChangesAsync();

//		//Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);


//		//Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
//		Task<bool> SoftDeleteAsync(int id);
//		Task<bool> ApproveProductAsync(int id, bool isApproved);



//	}
//}

using Tagerly.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;





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
        Task<bool> ApproveProductAsync(int id, bool isApproved);








    }
}