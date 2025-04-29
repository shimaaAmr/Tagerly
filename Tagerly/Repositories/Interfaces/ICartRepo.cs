using Tagerly.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Tagerly.Repositories.Interfaces
{
    public interface ICartRepo : IBaseRepo<Cart>
    {
        Task<Cart> GetUserCartAsync(string userId);
        Task<CartItem> GetCartItemAsync(int cartId, int productId);
        Task AddCartItemAsync(CartItem item);
        Task UpdateCartItemAsync(CartItem item);
        Task RemoveCartItemAsync(CartItem item);
    }
}