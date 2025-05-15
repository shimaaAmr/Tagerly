using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Tagerly.Services.Interfaces;
using Tagerly.Models.Enums;
using Tagerly.ViewModels;

namespace Tagerly.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly IEmailService _emailService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(
            IOrderService orderService,
            ICartService cartService,
            IEmailService emailService,
            ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _cartService = cartService;
            _emailService = emailService;
            _logger = logger;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);
        private string GetUserEmail() => User.FindFirstValue(ClaimTypes.Email);



        [HttpGet]
        public async Task<IActionResult> PlaceOrder()
        {
            var cart = await _cartService.GetUserCart(GetUserId());
            if (cart == null || !cart.CartItems.Any())
            {
                TempData["Warning"] = "Your cart is empty";
                return RedirectToAction("Index", "Cart");
            }

            // تمرير بيانات السلة إلى العرض
            ViewBag.Cart = cart;

            return View(new OrderViewModel
            {
                Email = GetUserEmail()
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(OrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var cart = await _cartService.GetUserCart(GetUserId());
                ViewBag.Cart = cart;
                return View(model);
            }

            try
            {
                var order = await _orderService.PlaceOrder(
                    GetUserId(),
                    model.PaymentMethod,
                    model.SelectedGovernorate,
                    model.Address,
                    model.Email,
                    model.Notes
                );


                await _emailService.SendEmailAsync(
                    model.Email,
                    "Order Confirmation",
                    $"Your order #{order.Id} has been placed."
                );

                return RedirectToAction("Confirmation", new { orderId = order.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Order placement failed");
                ModelState.AddModelError("", "Failed to place order. Please try again.");

                var cart = await _cartService.GetUserCart(GetUserId());
                ViewBag.Cart = cart;

                return View(model);
            }
        }
        public async Task<IActionResult> Confirmation(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);

            if (order == null || order.UserId != GetUserId())
                return NotFound();

            // التحقق من وجود Payment أو استخدام قيم افتراضية
            var viewModel = new OrderConfirmationViewModel
            {
                OrderId = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount, // استخدم القيمة المباشرة بدلاً من order.Payment.Amount
                Address = order.Address,
                PaymentMethod = order.PaymentMethod, // استخدم القيمة المباشرة
                Items = order.OrderDetails.Select(od => new OrderItemViewModel
                {
                    ProductId = od.ProductId,
                    ProductName = od.Product?.Name,
                    Quantity = od.Quantity,
                    Price = od.Price
                }).ToList()
            };

            return View(viewModel);
        }
    }
}