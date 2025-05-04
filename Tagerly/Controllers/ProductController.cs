using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tagerly.Services.Interfaces;
using Tagerly.ViewModels;
using AutoMapper;
using System.Security.Claims;

namespace Tagerly.Controllers
{
    public class ProductController : Controller
    {
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

        public async Task<IActionResult> Index(ProductFilterViewModel filterModel)
        {
            await LoadCategories(filterModel.CategoryId);

            var result = await _productService.GetFilteredProductsAsync(filterModel);
            ViewBag.FilterModel = filterModel;
            ViewBag.TotalPages = result.TotalPages;

            var productVMs = _mapper.Map<IEnumerable<ProductViewModel>>(result.Products);
            return View(productVMs);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            var productVM = _mapper.Map<ProductViewModel>(product);
            return View(productVM);
        }

        public async Task<IActionResult> Create()
        {
            await LoadCategories();
            return View(new ProductViewModel());
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(ProductViewModel productVM)
        //{
        //    RemoveUnboundModelFields();

        //    if (!ModelState.IsValid)
        //    {
        //        await LoadCategories(productVM.CategoryId);
        //        return View(productVM);
        //    }

        //    try
        //    {
        //        await _productService.AddProductAsync(productVM);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("", ex.Message);
        //        await LoadCategories(productVM.CategoryId);
        //        return View(productVM);
        //    }
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel productVM)
        {
            RemoveUnboundModelFields();

            // Set the current user as seller
            productVM.SellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            productVM.SellerName = User.Identity.Name;

            if (!ModelState.IsValid)
            {
                await LoadCategories(productVM.CategoryId);
                return View(productVM);
            }

            try
            {
                await _productService.AddProductAsync(productVM);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                await LoadCategories(productVM.CategoryId);
                return View(productVM);
            }
        }
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
            if (id != productVM.ProductId) return NotFound();

            RemoveUnboundModelFields(); 

            if (!ModelState.IsValid)
            {
                await LoadCategories(productVM.CategoryId);
                return View(productVM);
            }

            try
            {
                await _productService.UpdateProductAsync(productVM);
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
                ModelState.AddModelError("", ex.Message);
                await LoadCategories(productVM.CategoryId);
                return View(productVM);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            var productVM = _mapper.Map<ProductViewModel>(product);
            return View(productVM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction(nameof(Index));
        }

        #region Helpers
        private async Task LoadCategories(int? selectedId = null)
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", selectedId);
        }

        private void RemoveUnboundModelFields()
        {
            // These fields are auto-mapped and not directly bound in form
            ModelState.Remove(nameof(ProductViewModel.ImageUrl));
            ModelState.Remove(nameof(ProductViewModel.CategoryName));
            ModelState.Remove(nameof(ProductViewModel.SellerId));
            ModelState.Remove(nameof(ProductViewModel.SellerName));
        } 
        #endregion
    }
}
