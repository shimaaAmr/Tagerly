using Tagerly.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tagerly.Repositories.Interfaces
{
    public interface IUserRepo : IGenericRepo<ApplicationUser>
    {
        // يمكن إضافة دوال خاصة بالمستخدمين هنا
        Task<ApplicationUser> GetUserWithOrdersAsync(string userId);
    }
}