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
        #region Fields & Constructor
        private readonly IProductRepo _repository;
        private readonly IMapper _mapper;
        private readonly string _imageUploadPath;

        public ProductService(IProductRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _imageUploadPath = Path.Combine("wwwroot", "images");
            EnsureUploadDirectoryExists();
        }
        #endregion

        #region Initialization
        private void EnsureUploadDirectoryExists()
        {
            if (!Directory.Exists(_imageUploadPath))
            {
                Directory.CreateDirectory(_imageUploadPath);
            }
        }
        #endregion

        #region Public Methods
        public async Task<ProductPagedResult> GetFilteredProductsAsync(ProductFilterViewModel productFilterVM)
        {
            var filter = productFilterVM.BuildFilter();
            var orderBy = productFilterVM.BuildSort();

            var (products, totalCount) = await _repository.GetPagedAsync(
                productFilterVM.PageIndex,
                productFilterVM.PageSize,
                filter,
                orderBy);

            return MapToPagedResult(products, totalCount, productFilterVM);
        }

        public async Task<ProductViewModel> GetProductByIdAsync(int id)
        {
            var product = await _repository.GetByIdWithDetailsAsync(id);
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
            product.SellerId = productVM.SellerId;

            await _repository.AddAsync(product); 
            await _repository.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(ProductViewModel productVM)
        {
            var existingProduct = await GetExistingProductOrThrow(productVM.ProductId);
            UpdateProductWithImage(existingProduct, productVM);
            await SaveProductAsync(existingProduct);
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await GetExistingProductOrThrow(id);
            DeleteProductImage(product);
            await RemoveProductAsync(product);
        }
        #endregion

        #region Private Methods
        private ProductPagedResult MapToPagedResult(
            IEnumerable<Product> products,
            int totalCount,
            ProductFilterViewModel productFilterVM)
        {
            return new ProductPagedResult
            {
                Products = _mapper.Map<IEnumerable<ProductViewModel>>(products),
                TotalCount = totalCount,
                PageIndex = productFilterVM.PageIndex,
                PageSize = productFilterVM.PageSize
            };
        }

        private async Task<Product> GetExistingProductOrThrow(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            return product ?? throw new KeyNotFoundException("Product not found");
        }

        private void UpdateProductWithImage(Product existingProduct, ProductViewModel productVM)
        {
            var existingImageUrl = existingProduct.ImageUrl;
            _mapper.Map(productVM, existingProduct);

            if (productVM.ImageFile != null && productVM.ImageFile.Length > 0)
            {
                DeleteImageIfExists(existingImageUrl);
                existingProduct.ImageUrl = HandleImageUploadAsync(productVM.ImageFile).Result;
            }
            else
            {
                existingProduct.ImageUrl = existingImageUrl;
            }
        }

        private async Task SaveProductAsync(Product product)
        {
            _repository.Update(product);
            await _repository.SaveChangesAsync();
        }

        private async Task RemoveProductAsync(Product product)
        {
            _repository.Delete(product);
            await _repository.SaveChangesAsync();
        }

        private void DeleteProductImage(Product product)
        {
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                DeleteImageIfExists(product.ImageUrl);
            }
        }

        private async Task<string> HandleImageUploadAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) return null;

            var fileName = GenerateUniqueFileName(file.FileName);
            var filePath = GetFilePath(fileName);

            await SaveFileAsync(file, filePath);

            return $"/images/{fileName}";
        }

        private string GenerateUniqueFileName(string originalFileName)
        {
            return $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
        }

        private string GetFilePath(string fileName)
        {
            return Path.Combine(_imageUploadPath, fileName);
        }

        private async Task SaveFileAsync(IFormFile file, string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
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
        #endregion
    }
}