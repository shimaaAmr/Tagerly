using Microsoft.AspNetCore.Mvc;
using Tagerly.Services.Interfaces.Admin;

namespace Tagerly.Controllers
{
    public class AdminProductController : Controller
    {



        private readonly IAdminProductService _productService;

        public AdminProductController(IAdminProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }

        public async Task<IActionResult> Approve(int id)
        {
            await _productService.ApproveProductAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Reject(int id)
        {
            await _productService.RejectProductAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}