using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tagerly.DataAccess.DbContexts;
using Tagerly.Models;
using Tagerly.Services.Interfaces;
using Tagerly.ViewModels;
using Microsoft.Extensions.Logging;

namespace Tagerly.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly TagerlyDbContext _context;
        private readonly ILogger<CartService> _logger;

        public CartService(TagerlyDbContext context, ILogger<CartService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<CartViewModel> GetUserCart(string userId)
        {
            try
            {
                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null)
                {
                    return new CartViewModel
                    {
                        UserId = userId,
                        CartItems = new List<CartItemViewModel>()
                    };
                }

                return new CartViewModel
                {
                    CartId = cart.Id,
                    UserId = cart.UserId,
                    CartItems = cart.CartItems.Select(ci => new CartItemViewModel
                    {
                        Id = ci.Id,
                        ProductId = ci.ProductId,
                        ProductName = ci.Product?.Name ?? "Unknown Product",
                        ProductPrice = ci.Product?.Price ?? 0,
                        Quantity = ci.Quantity
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting user cart");
                return new CartViewModel
                {
                    UserId = userId,
                    CartItems = new List<CartItemViewModel>()
                };
            }
        }

        public async Task AddToCart(string userId, int productId, int quantity = 1)
        {
            try
            {
                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.UserId == userId) ?? await CreateNewCart(userId);

                var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                }
                else
                {
                    cart.CartItems.Add(new CartItem
                    {
                        ProductId = productId,
                        Quantity = quantity
                    });
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding product {productId} to cart for user {userId}");
                throw;
            }
        }

        public async Task RemoveFromCart(string userId, int cartItemId)
        {
            try
            {
                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null) return;

                var itemToRemove = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
                if (itemToRemove != null)
                {
                    _context.CartItems.Remove(itemToRemove);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error removing cart item {cartItemId} for user {userId}");
                throw;
            }
        }

        public async Task UpdateCartItem(string userId, int cartItemId, int newQuantity)
        {
            try
            {
                if (newQuantity <= 0)
                {
                    await RemoveFromCart(userId, cartItemId);
                    return;
                }

                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null) return;

                var itemToUpdate = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
                if (itemToUpdate != null)
                {
                    itemToUpdate.Quantity = newQuantity;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating quantity for cart item {cartItemId}");
                throw;
            }
        }

        public async Task ClearCart(string userId)
        {
            try
            {
                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null) return;

                _context.CartItems.RemoveRange(cart.CartItems);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error clearing cart for user {userId}");
                throw;
            }
        }

        public async Task<int> GetCartItemCount(string userId)
        {
            try
            {
                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                return cart?.CartItems.Sum(ci => ci.Quantity) ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting cart item count for user {userId}");
                return 0;
            }
        }

        private async Task<Cart> CreateNewCart(string userId)
        {
            var newCart = new Cart
            {
                UserId = userId,
                CartItems = new List<CartItem>()
            };

            await _context.Carts.AddAsync(newCart);
            await _context.SaveChangesAsync();

            return newCart;
        }
    }
}