using System;
using System.Linq;
using System.Threading.Tasks;
using Tagerly.Models;
using Tagerly.Models.Enums;
using Tagerly.Repositories.Interfaces;
using Tagerly.Services.Interfaces;

namespace Tagerly.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepo _orderRepo;
        private readonly ICartRepo _cartRepo;
        private readonly IProductRepo _productRepo;

        public OrderService(
            IOrderRepo orderRepo,
            ICartRepo cartRepo,
            IProductRepo productRepo)
        {
            _orderRepo = orderRepo;
            _cartRepo = cartRepo;
            _productRepo = productRepo;
        }

        public async Task<Order> PlaceOrder(
            string userId,
            PaymentMethod paymentMethod,
            string shippingAddress,
            string email)
        {
            var cart = await _cartRepo.GetUserCartAsync(userId);

            if (cart == null || !cart.CartItems.Any())
                throw new InvalidOperationException("Cart is empty");

            // حساب الإجمالي وإنشاء التفاصيل
            var orderDetails = cart.CartItems.Select(ci => new OrderDetail
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                Price = ci.Product.Price
            }).ToList();

            var payment = new Payment
            {
                Amount = orderDetails.Sum(od => od.Price * od.Quantity),
                Method = paymentMethod.ToString(),
                PaymentDate = DateTime.UtcNow
            };

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = "Processing",
                ShippingAddress = shippingAddress,
                OrderDetails = orderDetails,
                Payment = payment
            };

            await _orderRepo.AddAsync(order);
            await _orderRepo.SaveChangesAsync();

            // تحديث المخزون
            foreach (var item in cart.CartItems)
            {
                var product = await _productRepo.GetByIdAsync(item.ProductId);
                product.Quantity -= item.Quantity;
                //await _productRepo.UpdateAsync(product);
            }

            return order;
        }
    }
}