using Tagerly.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tagerly.Repositories.Interfaces
{
    public interface IProductRepo : IGenericRepo<Product>
    {
        // يمكن إضافة دوال خاصة بالمنتجات هنا
        Task<List<Product>> GetProductsByCategoryAsync(int categoryId);
        
    }
}