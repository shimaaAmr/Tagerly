using Tagerly.Models;
using System.Threading.Tasks;
using Tagerly.ViewModels;

namespace Tagerly.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartViewModel> GetUserCart(string userId);
        
        Task AddToCart(string userId, int productId, int quantity = 1);
        Task UpdateCartItem(string userId, int productId, int quantity);
        Task RemoveFromCart(string userId, int productId);
        Task ClearCart(string userId);
    }
}