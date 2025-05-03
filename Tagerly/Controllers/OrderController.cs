using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tagerly.Services.Interfaces;
using Tagerly.ViewModels;
using Tagerly.Models;
using Tagerly.Models.Enums;
using Microsoft.Extensions.Logging;

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

        [HttpGet]
        public async Task<IActionResult> PlaceOrder()
        {
            try
            {
                var cart = await _cartService.GetUserCart(GetUserId());

                if (cart?.CartItems == null || !cart.CartItems.Any())
                {
                    TempData["Warning"] = "Your cart is empty";
                    return RedirectToAction("Index", "Cart");
                }

                ViewBag.Cart = MapToViewModel(cart);
                return View(new CreateOrderViewModel
                {
                    Email = User.FindFirstValue(ClaimTypes.Email)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading order page");
                TempData["Error"] = "Error loading order page";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(CreateOrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please correct the errors";
                return View(model);
            }

            try
            {
                var order = await _orderService.PlaceOrder(
                    GetUserId(),
                    model.PaymentMethod,
                    model.ShippingAddress,
                    model.Email);

                await _cartService.ClearCart(GetUserId());

                await _emailService.SendEmailAsync(
                    model.Email,
                    $"Order Confirmation #{order.Id}",
                    GenerateEmailContent(order));

                return RedirectToAction("Confirmation", new { orderId = order.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Order processing failed");
                TempData["Error"] = ex.Message;
                return RedirectToAction("PlaceOrder");
            }
        }

        [HttpGet]
        public IActionResult Confirmation(int orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        private CartViewModel MapToViewModel(Cart cart)
        {
            return new CartViewModel
            {
                CartItems = cart.CartItems.Select(ci => new CartItemViewModel
                {
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.Name ?? "Unknown",
                    ProductPrice = ci.Product?.Price ?? 0,
                    Quantity = ci.Quantity,
                    ImageUrl = ci.Product?.ImageUrl ?? "/images/default-product.png"
                }).ToList()
            };
        }

        private string GenerateEmailContent(Order order)
        {
            return $@"<h1>Thank you for your order!</h1>
                     <p>Order Number: #{order.Id}</p>
                     <p>Date: {order.OrderDate:yyyy-MM-dd}</p>
                     <p>Total: {order.Payment.Amount:C}</p>";
        }
    }
}