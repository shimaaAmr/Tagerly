using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Tagerly.Repositories.Interfaces
{
    public interface IGenericRepo<T> where T : class
    {
        #region Basic CRUD
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveChangesAsync();
        #endregion

        #region Query Operations
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        #endregion

        #region Paging
        Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
            int pageIndex,
            int pageSize,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

        Task<IEnumerable<T>> GetFilteredAsync(
            Expression<Func<T, bool>> filterCondition,
            Expression<Func<T, bool>> skipCondition,
            Expression<Func<T, bool>> takeCondition);
        #endregion
    }
}