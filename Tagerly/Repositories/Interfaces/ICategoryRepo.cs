using System.Collections.Generic;
using System.Threading.Tasks;
using Tagerly.Models;

namespace Tagerly.Repositories.Interfaces
{
    public interface ICategoryRepo : IGenericRepo<Category>
    {
        #region Extended Read Operations
        Task<IEnumerable<Category>> GetAllWithProductsAsync();
        #endregion
    }
}