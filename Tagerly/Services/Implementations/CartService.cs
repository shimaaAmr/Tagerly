using Tagerly.Repositories.Interfaces;
using Tagerly.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tagerly.Services.Interfaces;

namespace Tagerly.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly ICartRepo _cartRepo;
        private readonly IProductRepo _productRepo;

        public CartService(ICartRepo cartRepo, IProductRepo productRepo)
        {
            _cartRepo = cartRepo;
            _productRepo = productRepo;
        }

        public async Task<Cart> GetUserCart(string userId)
        {
            return await _cartRepo.GetUserCartAsync(userId);
        }

        public async Task AddToCart(string userId, int productId, int quantity = 1)
        {
            var cart = await _cartRepo.GetUserCartAsync(userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                await _cartRepo.AddCartAsync(cart); // Add this method if it doesn’t exist
                await _cartRepo.SaveChangesAsync(); // عشان cart.Id يتولد
            }

            var product = await _productRepo.GetByIdAsync(productId);
            var existingItem = await _cartRepo.GetCartItemAsync(cart.Id, productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                await _cartRepo.UpdateCartItemAsync(existingItem);
            }
            else
            {
                await _cartRepo.AddCartItemAsync(new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity
                });
            }

            await _cartRepo.SaveChangesAsync();
        }


        public async Task UpdateCartItem(string userId, int productId, int quantity)
        {
            var cart = await _cartRepo.GetUserCartAsync(userId);
            var item = await _cartRepo.GetCartItemAsync(cart.Id, productId);

            if (item != null)
            {
                item.Quantity = quantity;
                await _cartRepo.UpdateCartItemAsync(item);
                await _cartRepo.SaveChangesAsync();
            }
        }

        public async Task ClearCart(string userId)
        {
            var cart = await _cartRepo.GetUserCartAsync(userId);

            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
                return;

            foreach (var item in cart.CartItems)
            {
                await _cartRepo.RemoveCartItemAsync(item);
            }

            await _cartRepo.SaveChangesAsync();
        }
        public async Task RemoveFromCart(string userId, int productId)
        {
            var cart = await _cartRepo.GetUserCartAsync(userId);
            var item = await _cartRepo.GetCartItemAsync(cart.Id, productId);

            if (item != null)
            {
                await _cartRepo.RemoveCartItemAsync(item);
                await _cartRepo.SaveChangesAsync();
            }
        }

    }
}