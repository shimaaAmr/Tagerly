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
            var payment = new Payment
            {
                Amount = await CalculateCartTotal(userId),
                Method = paymentMethod.ToString(),
                PaymentDate = DateTime.UtcNow
            };

            return await _orderRepo.CreateOrderFromCartAsync(userId, payment);
        }

        private async Task<decimal> CalculateCartTotal(string userId)
        {
            var cart = await _cartRepo.GetUserCartAsync(userId);
            return cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity);
        }
    }
}