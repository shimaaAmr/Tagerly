using System.Collections.Generic;
using System.Threading.Tasks;
using Tagerly.ViewModels;

namespace Tagerly.Services.Interfaces
{
    public interface ICategoryService
    {
        #region Read Operations
        Task<bool> CategoryExists(int id);
        Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync();
        Task<CategoryViewModel> GetCategoryByIdAsync(int id);
        #endregion

        #region Write Operations
        Task AddCategoryAsync(CategoryViewModel categoryViewModel);
        Task UpdateCategoryAsync(CategoryViewModel categoryViewModel);
        Task DeleteCategoryAsync(int id);
        #endregion
    }
}