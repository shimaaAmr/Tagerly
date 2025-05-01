using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Tagerly.Repositories.Interfaces
{
    public interface IGenericRepo<T> where T : class
    {
        Task AddAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        void Update(T entity);
        void Delete(T entity);
        Task SaveChangesAsync();
    }
}