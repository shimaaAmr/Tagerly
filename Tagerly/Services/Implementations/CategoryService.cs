using AutoMapper;
using NuGet.Protocol.Core.Types;
using Tagerly.Models;
using Tagerly.Repositories.Interfaces;
using Tagerly.Services.Interfaces;
using Tagerly.ViewModels;

namespace Tagerly.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepo _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepo categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<bool> CategoryExists(int id) =>
           await _categoryRepository.GetByIdAsync(id) != null;
        public async Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryViewModel>>(categories);
        }

        public async Task<CategoryViewModel> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return _mapper.Map<CategoryViewModel>(category);
        }

        public async Task AddCategoryAsync(CategoryViewModel categoryViewModel)
        {
            var category = _mapper.Map<Category>(categoryViewModel);
            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(CategoryViewModel categoryViewModel)
        {
            var category = _mapper.Map<Category>(categoryViewModel);
            _categoryRepository.Update(category);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category != null)
            {
                _categoryRepository.Delete(category);
                await _categoryRepository.SaveChangesAsync();
            }
        }
    }
}
