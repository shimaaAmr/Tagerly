using System.Threading.Tasks;
using Tagerly.ViewModels;

namespace Tagerly.Services.Interfaces
{
    public interface IProductService
    {
        #region Read Operations
        Task<ProductPagedResult> GetFilteredProductsAsync(ProductFilterViewModel productFilterVM);
        Task<ProductViewModel> GetProductByIdAsync(int id);
        Task<bool> ProductExists(int id);
        #endregion

        #region Write Operations
        Task AddProductAsync(ProductViewModel productVM);
        Task UpdateProductAsync(ProductViewModel productVM);
        Task DeleteProductAsync(int id);
        #endregion
    }
}