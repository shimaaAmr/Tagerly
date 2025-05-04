using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using AutoMapper;
using Tagerly.Models;
using Tagerly.Repositories.Interfaces;
using Tagerly.Services.Interfaces;
using Tagerly.ViewModels;

namespace Tagerly.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo _repository;
        private readonly IMapper _mapper;
        private readonly string _imageUploadPath = Path.Combine("wwwroot", "images");

        public ProductService(IProductRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            EnsureUploadDirectoryExists();
        }

        private void EnsureUploadDirectoryExists()
        {
            if (!Directory.Exists(_imageUploadPath))
                Directory.CreateDirectory(_imageUploadPath);
        }

        public async Task<ProductPagedResult> GetFilteredProductsAsync(ProductFilterViewModel productFilterVM)
        {
            var filter = productFilterVM.BuildFilter();
            var orderBy = productFilterVM.BuildSort();

            var (products, totalCount) = await _repository.GetPagedAsync(
                productFilterVM.PageIndex,
                productFilterVM.PageSize,
                filter,
                orderBy);

            return new ProductPagedResult
            {
                Products = _mapper.Map<IEnumerable<ProductViewModel>>(products),
                TotalCount = totalCount,
                PageIndex = productFilterVM.PageIndex,
                PageSize = productFilterVM.PageSize
            };
        }

        public async Task<ProductViewModel> GetProductByIdAsync(int id)
        {
            var product = await _repository.GetByIdWithCategoryAsync(id); 
            return product == null ? null : _mapper.Map<ProductViewModel>(product);
        }

        public async Task<bool> ProductExists(int id)
        {
            return await _repository.GetByIdAsync(id) != null;
        }

        public async Task AddProductAsync(ProductViewModel productVM)
        {
            var product = _mapper.Map<Product>(productVM);
            product.ImageUrl = await HandleImageUploadAsync(productVM.ImageFile);
            product.SellerId = productVM.SellerId; // Ensure seller ID is set

            await _repository.AddAsync(product);
            await _repository.SaveChangesAsync();
        }
        public async Task UpdateProductAsync(ProductViewModel productVM)
        {
            var existingProduct = await _repository.GetByIdAsync(productVM.ProductId)
                ?? throw new KeyNotFoundException("Product not found");

            // Preserve existing image if no new file is uploaded
            var existingImageUrl = existingProduct.ImageUrl;

            _mapper.Map(productVM, existingProduct);

            if (productVM.ImageFile != null && productVM.ImageFile.Length > 0)
            {
                // New image uploaded - delete old and save new
                DeleteImageIfExists(existingImageUrl);
                existingProduct.ImageUrl = await HandleImageUploadAsync(productVM.ImageFile);
            }
            else
            {
                // No new image - keep the existing one
                existingProduct.ImageUrl = existingImageUrl;
            }

            _repository.Update(existingProduct);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException("Product not found");

            DeleteImageIfExists(product.ImageUrl);
            _repository.Delete(product);
            await _repository.SaveChangesAsync();
        }

        private async Task<string> HandleImageUploadAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) return null;

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(_imageUploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/images/{fileName}";
        }

        private void DeleteImageIfExists(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl) || !imageUrl.StartsWith("/images/")) return;

            var imagePath = Path.Combine("wwwroot", imageUrl.TrimStart('/'));
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }
    }
}
