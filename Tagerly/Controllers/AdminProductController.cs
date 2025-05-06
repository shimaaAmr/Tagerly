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




        //=================================
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            return View(product); // يرجع View اسمه Delete.cshtml
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result) return NotFound();

            return RedirectToAction("Index");
        }

        //=================================================
        [HttpGet]
        public async Task<IActionResult> Approve(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            return View(product); // يرجع View اسمه Approve.cshtml
        }

        [HttpPost]
        public async Task<IActionResult> ApproveConfirmation(int id)
        {
            var result = await _productService.ChangeApprovalStatusAsync(id, true);
            if (!result) return NotFound();

            return RedirectToAction("Index");
        }
        //==============================
        [HttpGet]
        public async Task<IActionResult> Reject(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            return View(product); // يعرض View اسمه Reject.cshtml
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectConfirmation(int id)
        {
            var result = await _productService.ChangeApprovalStatusAsync(id, false);
            if (!result) return NotFound();

            return RedirectToAction("Index");
        }


        //[HttpPost]
        //public async Task<IActionResult> Reject(int id)
        //{
        //    var result = await _productService.ChangeApprovalStatusAsync(id, false);
        //    if (!result) return NotFound();

        //    return RedirectToAction("Index");
        //}
    }
}