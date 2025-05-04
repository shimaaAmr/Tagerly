using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tagerly.Services.Interfaces;
using System.Threading.Tasks;
using Tagerly.ViewModels;

namespace Tagerly.Controllers
{
    //[Authorize]
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
            var cartViewModel = await _cartService.GetUserCart(userId);

            return View(cartViewModel ?? new CartViewModel { UserId = userId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var userId = GetUserId();
            await _cartService.AddToCart(userId, productId, quantity);
            TempData["Success"] = "Product added to cart successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            var userId = GetUserId();
            await _cartService.RemoveFromCart(userId, cartItemId);
            TempData["Success"] = "Product removed from cart.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCart(int cartItemId, int quantity)
        {
            var userId = GetUserId();
            await _cartService.UpdateCartItem(userId, cartItemId, quantity);
            TempData["Success"] = "Cart updated successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClearCart()
        {
            var userId = GetUserId();
            await _cartService.ClearCart(userId);
            TempData["Success"] = "Cart cleared.";
            return RedirectToAction("Index");
        }
    }
}