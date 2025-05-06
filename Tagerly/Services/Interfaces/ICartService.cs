using System.Threading.Tasks;
using Tagerly.Models;
using Tagerly.ViewModels;

namespace Tagerly.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartViewModel> GetUserCart(string userId);
        Task AddToCart(string userId, int productId, int quantity = 1);
        Task RemoveFromCart(string userId, int cartItemId);
        Task UpdateCartItem(string userId, int cartItemId, int newQuantity);
        Task ClearCart(string userId);
        Task<int> GetCartItemCount(string userId);
        Task<Cart> CreateNewCartForUser(string userId);
    }
}