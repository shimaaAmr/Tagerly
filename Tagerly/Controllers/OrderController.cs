using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tagerly.Services.Interfaces;
using Tagerly.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Tagerly.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        // GET: /Order/MyOrders
        public async Task<IActionResult> MyOrders()
        {
            var userId = GetUserId();
            var orders = await _orderService.GetUserOrders(userId);
            return View(orders);
        }

        // POST: /Order/PlaceOrder
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(PaymentMethod method)
        {
            var userId = GetUserId();
            var order = await _orderService.PlaceOrder(userId, method);

            if (order == null)
                return RedirectToAction("Index", "Cart"); // لو الكارت فاضي

            return RedirectToAction("MyOrders");
        }
    }
}
