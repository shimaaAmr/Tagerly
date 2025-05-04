//using System.Linq;
//using System.Linq.Expressions;
//using Microsoft.EntityFrameworkCore;
//using Tagerly.DataAccess;
//using Tagerly.Models;
//using Tagerly.Repositories.Interfaces;

//namespace Tagerly.Repositories.Implementations
//{
//	public class ProductRepo : GenericRepo<Product>, IProductRepo
//	{
//		private readonly TagerlyDbContext _context;
//		private readonly DbSet<Product> _dbSet;

//		public ProductRepo(TagerlyDbContext context) : base(context)
//		{
//			_context = context;
//			_dbSet = context.Set<Product>();
//		}

//		Task<bool> IProductRepo.ApproveProductAsync(int id, bool isApproved)
//		{
//			throw new NotImplementedException();
//		}

//		Task<IEnumerable<Product>> IProductRepo.FindAsync(Expression<Func<Product, bool>> predicate)
//		{
//			throw new NotImplementedException();
//		}

//		Task<List<Product>> IProductRepo.GetAllBySellerRoleAsync()
//		{
//			throw new NotImplementedException();
//		}

//		Task<Product> IProductRepo.GetByIdWithCategoryAsync(int id)
//		{
//			throw new NotImplementedException();
//		}

//		Task<(IEnumerable<Product> Items, int TotalCount)> IProductRepo.GetPagedAsync(int pageIndex, int pageSize, Expression<Func<Product, bool>> filter, Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy)
//		{
//			throw new NotImplementedException();
//		}

//		Task<List<Product>> IProductRepo.GetProductsByCategoryAsync(int categoryId)
//		{
//			throw new NotImplementedException();
//		}

//		Task IProductRepo.SaveChangesAsync()
//		{
//			throw new NotImplementedException();
//		}

//		Task<bool> IProductRepo.SoftDeleteAsync(int id)
//		{
//			throw new NotImplementedException();
//		}

//		Task IProductRepo.UpdateAsync(Product product)
//		{
//			throw new NotImplementedException();
//		}
//		//        public async Task<Product> GetByIdWithCategoryAsync(int id)
//		//        {
//		//            return await _context.Products
//		//                .Include(p => p.Category)  // This ensures Category is loaded
//		//                .FirstOrDefaultAsync(p => p.Id == id);
//		//        }
//		//        public async Task<IEnumerable<Product>> FindAsync(Expression<Func<Product, bool>> predicate)
//		//        {
//		//            return await _dbSet
//		//                .Include(p => p.Category)
//		//                .Where(predicate)
//		//                .ToListAsync();
//		//        }

//		//        public async Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedAsync(
//		//            int pageIndex,
//		//            int pageSize,
//		//            Expression<Func<Product, bool>> filter = null,
//		//            Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy = null)
//		//        {
//		//            IQueryable<Product> query = _dbSet.Include(p => p.Category);

//		//            if (filter != null)
//		//            {
//		//                query = query.Where(filter);
//		//            }

//		//            var totalCount = await query.CountAsync();

//		//            if (orderBy != null)
//		//            {
//		//                query = orderBy(query);
//		//            }

//		//            var items = await query
//		//                .Skip((pageIndex - 1) * pageSize)
//		//                .Take(pageSize)
//		//                .ToListAsync();

//		//            return (items, totalCount);
//		//        }

//		//        public async Task<List<Product>> GetAllBySellerRoleAsync()
//		//        {
//		//            var sellerRoleId = await _context.Roles
//		//                .Where(r => r.Name == "Seller")
//		//                .Select(r => r.Id)
//		//                .FirstOrDefaultAsync();

//		//            return await _context.Products


//		//=======
//		//                .Include(p => p.Category)  // This ensures Category is loaded
//		//                .FirstOrDefaultAsync(p => p.Id == id);
//		//        }
//		//        public async Task<IEnumerable<Product>> FindAsync(Expression<Func<Product, bool>> predicate)
//		//        {
//		//            return await _dbSet
//		//                .Include(p => p.Category)
//		//                .Where(predicate)
//		//                .ToListAsync();
//		//        }

//		//        public async Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedAsync(
//		//            int pageIndex,
//		//            int pageSize,
//		//            Expression<Func<Product, bool>> filter = null,
//		//            Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy = null)
//		//        {
//		//            IQueryable<Product> query = _dbSet.Include(p => p.Category);

//		//            if (filter != null)
//		//            {
//		//                query = query.Where(filter);
//		//            }

//		//            var totalCount = await query.CountAsync();

//		//            if (orderBy != null)
//		//            {
//		//                query = orderBy(query);
//		//            }

//		//            var items = await query
//		//                .Skip((pageIndex - 1) * pageSize)
//		//                .Take(pageSize)
//		//                .ToListAsync();

//		//            return (items, totalCount);
//		//        }

//		//        public async Task UpdateAsync(Product product)
//		//        {
//		//            _context.Products.Update(product);
//		//            await Task.CompletedTask; // ليتوافق مع التوازع async
//		//        }

//		//        public async Task SaveChangesAsync()
//		//        {
//		//            await _context.SaveChangesAsync();
//		//        }

//		//        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
//		//        {
//		//            return await _dbSet
//		//                .Include(p => p.Category)


//		//                .Include(p => p.Seller)
//		//                .Where(p => _context.UserRoles
//		//                    .Any(ur => ur.UserId == p.SellerId && ur.RoleId == sellerRoleId))
//		//                .ToListAsync();
//		//        }


//		//   public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
//		//        {
//		//            return await _dbSet
//		//                .Include(p => p.Category)

//		//                .Where(p => p.CategoryId == categoryId)
//		//                .ToListAsync();
//		//        }
//		//        //==================================================================
//		//        public async Task<bool> SoftDeleteAsync(int id)
//		//        {
//		//            var product = await _context.Products.FindAsync(id);
//		//            if (product == null) return false;

//		//            product.IsDeleted = true;
//		//            _context.Products.Update(product);
//		//            await _context.SaveChangesAsync();
//		//            return true;
//		//        }

//		//        public async Task<bool> ApproveProductAsync(int id, bool isApproved)
//		//        {
//		//            var product = await _context.Products.FindAsync(id);
//		//            if (product == null) return false;

//		//            product.IsApproved = isApproved;
//		//            _context.Products.Update(product);
//		//            await _context.SaveChangesAsync();
//		//            return true;
//		//        }

//	}
//}

using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Tagerly.DataAccess;
using Tagerly.Models;
using Tagerly.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Tagerly.Repositories.Implementations
{
	public class ProductRepo : GenericRepo<Product>, IProductRepo
	{
		private readonly TagerlyDbContext _context;
		private readonly DbSet<Product> _dbSet;

		public ProductRepo(TagerlyDbContext context) : base(context)
		{
			_context = context;
			_dbSet = context.Set<Product>();
		}
		public async Task<Product> GetByIdWithCategoryAsync(int id)
		{
			return await _context.Products
				.Include(p => p.Category)  // This ensures Category is loaded
				.FirstOrDefaultAsync(p => p.Id == id);
		}
		public async Task<IEnumerable<Product>> FindAsync(Expression<Func<Product, bool>> predicate)
		{
			return await _dbSet
				.Include(p => p.Category)
				.Where(predicate)
				.ToListAsync();
		}

		public async Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedAsync(
			int pageIndex,
			int pageSize,
			Expression<Func<Product, bool>> filter = null,
			Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy = null)
		{
			IQueryable<Product> query = _dbSet.Include(p => p.Category);

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


		public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
		{
			return await _dbSet
				.Include(p => p.Category)
				.Where(p => p.CategoryId == categoryId)
				.ToListAsync();
		}


		public async Task UpdateAsync(Product product)
		{
			_context.Products.Update(product);
			await Task.CompletedTask; // ليتوافق مع التوازع async
		}
		public async Task<bool> SoftDeleteAsync(int id)
		{
			var product = await _context.Products.FindAsync(id);
			if (product == null) return false;

			product.IsDeleted = true;
			_context.Products.Update(product);
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<bool> ApproveProductAsync(int id, bool isApproved)
		{
			var product = await _context.Products.FindAsync(id);
			if (product == null) return false;

			product.IsApproved = isApproved;
			_context.Products.Update(product);
			await _context.SaveChangesAsync();
			return true;
		}



		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}


	}
}