using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tagerly.DataAccess;
using Tagerly.Models;
using Tagerly.Services.Interfaces;
using Tagerly.ViewModels;
using Microsoft.Extensions.Logging;
using Tagerly.DataAccess.DbContexts;

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
                // Look up the user first to get the CartId
                var user = await _context.Users
                    .Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                // If user has no cart, create one
                if (user?.Cart == null)
                {
                    await CreateNewCartForUser(userId);

                    // Refresh user with the new cart
                    user = await _context.Users
                        .Include(u => u.Cart)
                        .ThenInclude(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                        .FirstOrDefaultAsync(u => u.Id == userId);
                }

                return new CartViewModel
                {
                    CartId = user.Cart.Id,
                    UserId = userId,
                    CartItems = user.Cart.CartItems?.Select(ci => new CartItemViewModel
                    {
                        Id = ci.Id,
                        ProductId = ci.ProductId,
                        ProductName = ci.Product?.Name ?? "Unknown Product",
                        ProductDescription = ci.Product?.Description,
                        ImageUrl = ci.Product?.ImageUrl,
                        ProductPrice = ci.Product?.Price ?? 0,
                        Quantity = ci.Quantity
                    }).ToList() ?? new List<CartItemViewModel>()
                };
                // No need to set SubTotal, Total, or TotalItems as they're calculated properties
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
                // Get user with cart
                var user = await _context.Users
                    .Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                // Create cart if it doesn't exist
                if (user?.Cart == null)
                {
                    await CreateNewCartForUser(userId);

                    // Refresh user with the new cart
                    user = await _context.Users
                        .Include(u => u.Cart)
                        .ThenInclude(c => c.CartItems)
                        .FirstOrDefaultAsync(u => u.Id == userId);
                }

                var existingItem = user.Cart.CartItems?.FirstOrDefault(ci => ci.ProductId == productId);

                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                }
                else
                {
                    if (user.Cart.CartItems == null)
                    {
                        user.Cart.CartItems = new List<CartItem>();
                    }

                    user.Cart.CartItems.Add(new CartItem
                    {
                        ProductId = productId,
                        Quantity = quantity,
                        CartId = user.Cart.Id
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
                // Get user with cart
                var user = await _context.Users
                    .Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user?.Cart == null) return;

                var itemToRemove = user.Cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
                if (itemToRemove != null)
                {
                    _context.CartItems.Remove(itemToRemove);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _logger.LogWarning($"Cart item with ID {cartItemId} not found for user {userId}");
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

                // Get user with cart
                var user = await _context.Users
                    .Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user?.Cart == null) return;

                var itemToUpdate = user.Cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
                if (itemToUpdate != null)
                {
                    itemToUpdate.Quantity = newQuantity;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _logger.LogWarning($"Cart item with ID {cartItemId} not found for user {userId}");
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
                // Get user with cart
                var user = await _context.Users
                    .Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user?.Cart == null) return;

                _context.CartItems.RemoveRange(user.Cart.CartItems);
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
                // Get user with cart
                var user = await _context.Users
                    .Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                return user?.Cart?.CartItems?.Sum(ci => ci.Quantity) ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting cart item count for user {userId}");
                return 0;
            }
        }

        public async Task<Cart> CreateNewCartForUser(string userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Check if the user already has a cart
                var user = await _context.Users
                    .Include(u => u.Cart)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    throw new InvalidOperationException($"User with ID {userId} not found");
                }

                // If user already has a cart, return it
                if (user.Cart != null)
                {
                    return user.Cart;
                }

                // Create a new cart
                var newCart = new Cart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };

                // Save cart first to get its ID
                await _context.Carts.AddAsync(newCart);
                await _context.SaveChangesAsync();

                // Update user with CartId
                user.CartId = newCart.Id;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                _logger.LogInformation($"Created new cart (ID: {newCart.Id}) for user {userId}");
                return newCart;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, $"Error creating new cart for user {userId}");
                throw;
            }
        }
    }
}