using Tagerly.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tagerly.Repositories.Interfaces
{
    public interface ICategoryRepo : IBaseRepo<Category>
    {
        // يمكن إضافة دوال خاصة بالتصنيفات هنا
        Task<List<Category>> GetCategoriesWithProductsAsync();
    }
}