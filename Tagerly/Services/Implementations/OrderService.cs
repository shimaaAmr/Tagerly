using Tagerly.Repositories.Interfaces;
using Tagerly.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Tagerly.Models.Enums;
using Tagerly.Services.Interfaces;

namespace Tagerly.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepo _orderRepo;
        private readonly ICartRepo _cartRepo;

        public OrderService(IOrderRepo orderRepo, ICartRepo cartRepo)
        {
            _orderRepo = orderRepo;
            _cartRepo = cartRepo;
        }

        public async Task<List<Order>> GetUserOrders(string userId)
        {
            return await _orderRepo.GetUserOrdersAsync(userId);
        }

        public async Task<Order> PlaceOrder(string userId, PaymentMethod paymentMethod)
        {
            var cart = await _cartRepo.GetUserCartAsync(userId);
            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
                return null;

            var total = cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity);

            var payment = new Payment
            {
                Amount = total,
                Method = paymentMethod.ToString(),
                PaymentDate = DateTime.UtcNow
            };

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                Payment = payment,
                OrderDetails = cart.CartItems.Select(ci => new OrderDetail
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    Price = ci.Product.Price
                }).ToList()
            };

            await _orderRepo.AddAsync(order);
            await _orderRepo.SaveChangesAsync(); // حفظ الأوردر والدفع

            await _cartRepo.ClearCartAsync(cart);
            await _cartRepo.SaveChangesAsync(); // حفظ مسح الكارت

            return order;
        }
    }
}
