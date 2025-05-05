using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tagerly.DataAccess.DbContexts;
using Tagerly.Repositories.Interfaces;

namespace Tagerly.Repositories.Implementations
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        protected readonly TagerlyDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepo(TagerlyDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetPagedAsync(
            int pageIndex,
            int pageSize,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetFilteredAsync(
            Expression<Func<T, bool>> filterCondition,
            Expression<Func<T, bool>> skipCondition,
            Expression<Func<T, bool>> takeCondition)
        {
            IQueryable<T> query = _dbSet;

            if (filterCondition != null)
            {
                query = query.Where(filterCondition);
            }

            query = query.SkipWhile(skipCondition).TakeWhile(takeCondition);

            return await query.ToListAsync();
        }
    }
}