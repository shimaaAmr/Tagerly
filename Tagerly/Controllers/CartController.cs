using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tagerly.Services.Interfaces;
using System.Threading.Tasks;
using Tagerly.Models;
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
            // مؤقتًا نرجع ID ثابت لتجربة الكارت من غير لوجين
            return "dummy-user";
        }


        public async Task<IActionResult> Index()
        {

            var userId = GetUserId();
            var cart = await _cartService.GetUserCart(userId);

            var viewModel = MapToViewModel(cart);
            return View(viewModel);
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
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var userId = GetUserId();
            await _cartService.RemoveFromCart(userId, productId);
            TempData["Success"] = "Product removed from cart.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCart(int productId, int quantity)
        {
            var userId = GetUserId();
            await _cartService.UpdateCartItem(userId, productId, quantity);
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
        private CartViewModel MapToViewModel(Cart cart)
        {
            return new CartViewModel
            {
                UserId = cart.UserId,
                CartItems = cart.CartItems.Select(item => new CartItemViewModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    ProductName = item.Product?.Name, // لو عندك Include للمنتج
                    ProductPrice = item.Product?.Price ?? 0
                }).ToList()
            };
        }

    }

}

