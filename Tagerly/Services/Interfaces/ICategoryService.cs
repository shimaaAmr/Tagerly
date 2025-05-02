using Tagerly.ViewModels;

namespace Tagerly.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<bool> CategoryExists(int id);
        Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync();
        Task<CategoryViewModel> GetCategoryByIdAsync(int id);
        Task AddCategoryAsync(CategoryViewModel categoryViewModel);
        Task UpdateCategoryAsync(CategoryViewModel categoryViewModel);
        Task DeleteCategoryAsync(int id);
    }
}
