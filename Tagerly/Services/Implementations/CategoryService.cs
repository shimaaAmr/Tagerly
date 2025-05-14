using AutoMapper;
using Tagerly.Models;
using Tagerly.Repositories.Interfaces;
using Tagerly.Services.Interfaces;
using Tagerly.ViewModels;

namespace Tagerly.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        #region Fields & Constructor
        private readonly ICategoryRepo _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepo categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        #endregion

        #region Public Methods
        public async Task<bool> CategoryExists(int id)
        {
            return await _categoryRepository.GetByIdAsync(id) != null;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return MapToViewModelList(categories);
        }

        public async Task<CategoryViewModel> GetCategoryByIdAsync(int id)
        {
            var category = await GetCategoryOrNullAsync(id);
            return MapToViewModel(category);
        }

        public async Task AddCategoryAsync(CategoryViewModel categoryViewModel)
        {
            var category = MapToEntity(categoryViewModel);
            await AddAndSaveCategoryAsync(category);
        }

        public async Task UpdateCategoryAsync(CategoryViewModel categoryViewModel)
        {
            var category = MapToEntity(categoryViewModel);
            await UpdateAndSaveCategoryAsync(category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await GetCategoryOrNullAsync(id);
            if (category != null)
            {
                await DeleteAndSaveCategoryAsync(category);
            }
        }
        #endregion

        #region Private Methods
        private async Task<Category> GetCategoryOrNullAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        private CategoryViewModel MapToViewModel(Category category)
        {
            return category == null ? null : _mapper.Map<CategoryViewModel>(category);
        }

        private IEnumerable<CategoryViewModel> MapToViewModelList(IEnumerable<Category> categories)
        {
            return _mapper.Map<IEnumerable<CategoryViewModel>>(categories);
        }

        private Category MapToEntity(CategoryViewModel categoryViewModel)
        {
            return _mapper.Map<Category>(categoryViewModel);
        }

        private async Task AddAndSaveCategoryAsync(Category category)
        {
            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();
        }

        private async Task UpdateAndSaveCategoryAsync(Category category)
        {
            _categoryRepository.Update(category);
            await _categoryRepository.SaveChangesAsync();
        }

        private async Task DeleteAndSaveCategoryAsync(Category category)
        {
            _categoryRepository.Delete(category);
            await _categoryRepository.SaveChangesAsync();
        }
        #endregion
    }
}