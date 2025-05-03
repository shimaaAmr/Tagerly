using Tagerly.Models;
using System.Threading.Tasks;

namespace Tagerly.Repositories.Interfaces
{
    public interface ICartRepo
    {
        Task<Cart> GetUserCartAsync(string userId);
        Task<CartItem> GetCartItemAsync(int cartId, int productId);
        Task AddCartItemAsync(CartItem item);
        Task UpdateCartItemAsync(CartItem item);
        Task RemoveCartItemAsync(CartItem item);
        Task AddCartAsync(Cart cart);
        Task SaveChangesAsync();
        Task ClearCartAsync(Cart cart);
    }
}