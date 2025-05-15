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
            Governorate selectedGovernorate,
            string Address,
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
                var productUpdates = new Dictionary<int, int>(); // ProductId -> Quantity to decrease

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

                    // Store quantity updates for later processing
                    productUpdates[cartItem.ProductId] = cartItem.Quantity;
                }
                var subTotal = orderDetails.Sum(i => i.Quantity * i.Price);
                var sellerFee = subTotal * 0.10m; // 10 % من البيعه
                var sellerNet = subTotal - sellerFee;
                // 3. Create and save order
                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.UtcNow,
                    Status = "Pending",
                    SelectedGovernorate = selectedGovernorate,
                    Address = Address,
                    PaymentMethod = paymentMethod.ToString(),
                    Email = email,
                    Notes = notes,
                    OrderDetails = orderDetails,
                    TotalAmount = subTotal , // لو فيه ضريبة على المشتري

                    SubTotal = subTotal,
                    SellerFee = sellerFee,
                    SellerNet = sellerNet,
                    Payment = new Payment

                    {
                        Amount = totalAmount,
                        Method = paymentMethod.ToString(),
                        PaymentDate = DateTime.UtcNow,
                        TransactionId = Guid.NewGuid().ToString(), // Generate a proper transaction ID
                        UserId = userId,
                        Status = "Pending"

                    }
                };

                await _orderRepo.AddAsync(order);
                await _orderRepo.SaveChangesAsync();

                // 4. Update product quantities with retry logic
                foreach (var entry in productUpdates)
                {
                    int productId = entry.Key;
                    int quantityToDecrease = entry.Value;
                    bool updateSuccessful = false;
                    int retryCount = 0;
                    const int maxRetries = 3;

                    while (!updateSuccessful && retryCount < maxRetries)
                    {
                        try
                        {
                            // Get fresh product data
                            var freshProduct = await _productRepo.GetByIdAsync(productId);

                            if (freshProduct == null)
                            {
                                throw new InvalidOperationException($"Product {productId} not found during quantity update");
                            }

                            if (freshProduct.Quantity < quantityToDecrease)
                            {
                                throw new InvalidOperationException($"Not enough stock for product {productId}");
                            }

                            // Update quantity
                            freshProduct.Quantity -= quantityToDecrease;
                            await _productRepo.UpdateAsync(freshProduct);
                            await _productRepo.SaveChangesAsync();

                            updateSuccessful = true;
                        }
                        catch (Exception ex) when (IsOptimisticConcurrencyException(ex))
                        {
                            retryCount++;
                            if (retryCount >= maxRetries)
                            {
                                _logger.LogError(ex, $"Failed to update product {productId} quantity after {maxRetries} attempts");
                                throw;
                            }

                            _logger.LogWarning($"Concurrency conflict updating product {productId}, retry {retryCount}");
                            await Task.Delay(100 * retryCount); // Exponential backoff
                        }
                    }
                }

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

        private bool IsOptimisticConcurrencyException(Exception ex)
        {
            // This is a simplified check. You might need to adjust it based on
            // the specific exceptions thrown by your ORM
            return ex.Message.Contains("concurrency") ||
                   ex.Message.Contains("affected 0 row(s)") ||
                   ex.InnerException != null && IsOptimisticConcurrencyException(ex.InnerException);
        }
    }

    public class OrderProcessingException : Exception
    {
        public OrderProcessingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}