using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tagerly.Services.Interfaces;
using Tagerly.ViewModels;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Tagerly.Controllers
{
    [Authorize(Roles = "Buyer")]
    public class BuyerController : Controller
    {
        #region Fields & Constructor
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public BuyerController(
            IProductService productService,
            ICategoryService categoryService,
            IMapper mapper)
        {
            _productService = productService;
            _categoryService = categoryService;
            _mapper = mapper;
        }
        #endregion

        #region Public Actions
        public async Task<IActionResult> Index(ProductFilterViewModel productFilterVM)
        {
            // Only show approved products for buyers
            productFilterVM.IsApproved = true;
            productFilterVM.PageSize = 12; // Set default page size

            await LoadCategories(productFilterVM.CategoryId);
            var result = await _productService.GetFilteredProductsAsync(productFilterVM);

            // Pass pagination data to view
            ViewBag.FilterModel = productFilterVM;
            ViewBag.CurrentPage = productFilterVM.PageIndex;
            ViewBag.TotalPages = result.TotalPages;

            return View(_mapper.Map<IEnumerable<ProductViewModel>>(result.Products));
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
            	return NotFound();
            }
            //Only show approved products to buyers
            //if (product == null || product.IsApproved != true)
            //{
            //	return NotFound();
            //}

            return View(_mapper.Map<ProductViewModel>(product));
        }
        #endregion

        #region Private Helpers
        private async Task LoadCategories(int? selectedId = null)
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", selectedId);
        }
        #endregion
    }
}