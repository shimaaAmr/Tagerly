using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tagerly.Services.Interfaces;
using Tagerly.ViewModels;
using AutoMapper;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Tagerly.Controllers
{
    [Authorize(Roles = "Seller")]
    public class ProductController : Controller
    {
        #region Fields & Constructor
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public ProductController(
            IProductService productService,
            ICategoryService categoryService,
            IMapper mapper)
        {
            _productService = productService;
            _categoryService = categoryService;
            _mapper = mapper;
        }
        #endregion

        #region Product Views
        public async Task<IActionResult> Index(ProductFilterViewModel productFilterVM)
        {
            productFilterVM.SellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await LoadCategories(productFilterVM.CategoryId);

            var result = await _productService.GetFilteredProductsAsync(productFilterVM);
            ViewBag.FilterModel = productFilterVM;
            ViewBag.TotalPages = result.TotalPages;

            return View(_mapper.Map<IEnumerable<ProductViewModel>>(result.Products));
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Product not found!";
                return RedirectToAction(nameof(Index));
            }

            if (TempData["FromEdit"] != null)
            {
                TempData["InfoMessage"] = "Product was updated successfully!";
            }

            return View(_mapper.Map<ProductViewModel>(product));
        }
        #endregion

        #region Create Operations
        public async Task<IActionResult> Create()
        {
            await LoadCategories();
            return View(new ProductViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel productVM)
        {
            RemoveUnboundModelFields();
            productVM.SellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!ModelState.IsValid)
            {
                await LoadCategories(productVM.CategoryId);
                return View(productVM);
            }

            try
            {
                await _productService.AddProductAsync(productVM);
                TempData["SuccessMessage"] = $"Product '{productVM.Name}' has been added successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to add product: {ex.Message}";
                await LoadCategories(productVM.CategoryId);
                return View(productVM);
            }
        }
        #endregion

        #region Update Operations
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            var productVM = _mapper.Map<ProductViewModel>(product);
            await LoadCategories(productVM.CategoryId);

            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductViewModel productVM)
        {
            RemoveUnboundModelFields();
            productVM.SellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id != productVM.ProductId) return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadCategories(productVM.CategoryId);
                return View(productVM);
            }

            try
            {
                await _productService.UpdateProductAsync(productVM);
                TempData["SuccessMessage"] = $"Product '{productVM.Name}' has been updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _productService.ProductExists(id))
                    return NotFound();
                throw;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to update product: {ex.Message}";
                await LoadCategories(productVM.CategoryId);
                return View(productVM);
            }
        }
        #endregion

        #region Delete Operations
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            return View(_mapper.Map<ProductViewModel>(product));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null) return NotFound();

                await _productService.DeleteProductAsync(id);
                TempData["SuccessMessage"] = $"Product '{product.Name}' has been deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to delete product: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Helper Methods
        private async Task LoadCategories(int? selectedId = null)
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", selectedId);
        }

        private void RemoveUnboundModelFields()
        {
            ModelState.Remove(nameof(ProductViewModel.ImageUrl));
            ModelState.Remove(nameof(ProductViewModel.ImageFile));
            ModelState.Remove(nameof(ProductViewModel.CategoryName));
            ModelState.Remove(nameof(ProductViewModel.SellerId));
            ModelState.Remove(nameof(ProductViewModel.SellerName));
        }
        #endregion
    }
}