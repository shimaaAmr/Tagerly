using Tagerly.Models;
using System.Threading.Tasks;

namespace Tagerly.Services.Interfaces
{
    public interface ICartService
    {
        Task<Cart> GetUserCart(string userId);
        Task AddToCart(string userId, int productId, int quantity = 1);
        Task UpdateCartItem(string userId, int productId, int quantity);
        Task RemoveFromCart(string userId, int productId);
        Task ClearCart(string userId);
    }
}