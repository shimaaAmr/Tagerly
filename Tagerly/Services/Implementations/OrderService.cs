using System;
using System.Linq;
using System.Threading.Tasks;
using Tagerly.Models;
using Tagerly.Models.Enums;
using Tagerly.Repositories.Interfaces;
using Tagerly.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Tagerly.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepo _orderRepo;
        private readonly ICartService _cartService;
        private readonly IProductRepo _productRepo;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IOrderRepo orderRepo,
            ICartService cartService,
            IProductRepo productRepo,
            ILogger<OrderService> logger)
        {
            _orderRepo = orderRepo;
            _cartService = cartService;
            _productRepo = productRepo;
            _logger = logger;
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _orderRepo.GetOrderByIdWithDetails(orderId);
        }

        public async Task<Order> PlaceOrder(
            string userId,
            PaymentMethod paymentMethod,
            string shippingAddress,
            string billingAddress,
            string email,
            string notes)
        {
            using var transaction = await _orderRepo.BeginTransactionAsync();

            try
            {
                _logger.LogInformation($"Starting order placement for user {userId}");

                // 1. Get user cart with items
                var cart = await _cartService.GetUserCart(userId);
                if (cart == null || !cart.CartItems.Any())
                {
                    throw new InvalidOperationException("Cart is empty");
                }

                // 2. Validate products and calculate total
                decimal totalAmount = 0;
                var orderDetails = new List<OrderDetail>();

                foreach (var cartItem in cart.CartItems)
                {
                    var product = await _productRepo.GetByIdAsync(cartItem.ProductId);
                    if (product == null)
                    {
                        throw new InvalidOperationException($"Product {cartItem.ProductId} not found");
                    }

                    if (product.Quantity < cartItem.Quantity)
                    {
                        throw new InvalidOperationException($"Not enough stock for {product.Name}");
                    }

                    orderDetails.Add(new OrderDetail
                    {
                        ProductId = cartItem.ProductId,
                        Quantity = cartItem.Quantity,
                        Price = product.Price
                    });

                    totalAmount += product.Price * cartItem.Quantity;
                }

                // 3. Create and save order
                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.UtcNow,
                    Status = "Processing",
                    ShippingAddress = shippingAddress,
                    BillingAddress = billingAddress,
                    PaymentMethod = paymentMethod.ToString(),
                    Email = email,
                    Notes = notes,
                    TotalAmount = totalAmount,
                    OrderDetails = orderDetails,
                    Payment = new Payment
                    {
                        Amount = totalAmount,
                        Method = paymentMethod.ToString(),
                        PaymentDate = DateTime.UtcNow,
                      
                    }
                };

            

                await _orderRepo.AddAsync(order);
                await _orderRepo.SaveChangesAsync();

                // 4. Update product quantities
                foreach (var detail in orderDetails)
                {
                    var product = await _productRepo.GetByIdAsync(detail.ProductId);
                    product.Quantity -= detail.Quantity;
                    await _productRepo.UpdateAsync(product);
                }
                await _productRepo.SaveChangesAsync();

                // 5. Clear cart
                await _cartService.ClearCart(userId);

                // Commit transaction
                await transaction.CommitAsync();

                _logger.LogInformation($"Order #{order.Id} placed successfully");
                return order;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Order placement failed");
                throw new OrderProcessingException("Failed to place order. Please try again.", ex);
            }
        }

        // Additional helper methods would go here...
    }

    public class OrderProcessingException : Exception
    {
        public OrderProcessingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}