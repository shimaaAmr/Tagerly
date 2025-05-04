using System.Collections.Generic;
using System.Threading.Tasks;
using Tagerly.Models;
using Tagerly.ViewModels;

namespace Tagerly.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductPagedResult> GetFilteredProductsAsync(ProductFilterViewModel productFilterVM);
        Task<ProductViewModel> GetProductByIdAsync(int id);
        Task<bool> ProductExists(int id);
        Task AddProductAsync(ProductViewModel productVM);
        Task UpdateProductAsync(ProductViewModel productVM);
        Task DeleteProductAsync(int id);

    }
}