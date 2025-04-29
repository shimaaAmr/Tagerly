using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tagerly.Repositories.Interfaces
{
    public interface IBaseRepo<T> where T : class
    {
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task SaveAsync();
    }
}