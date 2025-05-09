using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tagerly.Services.Interfaces;
using System.Threading.Tasks;
using Tagerly.ViewModels;

namespace Tagerly.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                // Handle anonymous user (could redirect to login or implement guest cart)
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Index", "Cart") });
            }

            var cartViewModel = await _cartService.GetUserCart(userId);
            return View(cartViewModel ?? new CartViewModel { UserId = userId });
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(int productId, int quantity = 1)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "Please login to add items to cart" });
            }

            try
            {
                await _cartService.AddToCart(userId, productId, quantity);
                return Json(new { success = true, message = "Item added to cart successfully" });
            }
            catch (System.Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Index", "Cart") });
            }

            await _cartService.AddToCart(userId, productId, quantity);
            TempData["Success"] = "Product added to cart successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            await _cartService.RemoveFromCart(userId, cartItemId);
            TempData["Success"] = "Product removed from cart.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCart(int cartItemId, int quantity)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            await _cartService.UpdateCartItem(userId, cartItemId, quantity);
            TempData["Success"] = "Cart updated successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClearCart()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            await _cartService.ClearCart(userId);
            TempData["Success"] = "Cart cleared.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetItemCount()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Json(0);
            }

            var cartViewModel = await _cartService.GetUserCart(userId);
            int itemCount = cartViewModel?.CartItems?.Sum(i => i.Quantity) ?? 0;
            return Json(itemCount);
        }
    }
}