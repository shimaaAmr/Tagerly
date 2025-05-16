using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tagerly.Services.Interfaces;
using Tagerly.ViewModels;

namespace Tagerly.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminCategoryController : Controller
	{
		private readonly ICategoryService _categoryService;

		public AdminCategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		#region Index
		public async Task<IActionResult> Index()
		{
			var categories = await _categoryService.GetAllCategoriesAsync();
			return View(categories);
		} 
		#endregion

		#region Details
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null) return NotFound();

			var category = await _categoryService.GetCategoryByIdAsync(id.Value);
			return category == null ? NotFound() : View(category);
		} 
		#endregion

		#region Create
		public IActionResult Create() => View();

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CategoryViewModel categoryViewModel)
		{
			if (!ModelState.IsValid) return View(categoryViewModel);

			await _categoryService.AddCategoryAsync(categoryViewModel);
			return RedirectToAction(nameof(Index));
		} 
		#endregion

		#region Edit
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null) return NotFound();

			var category = await _categoryService.GetCategoryByIdAsync(id.Value);
			return category == null ? NotFound() : View(category);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, CategoryViewModel categoryViewModel)
		{
			if (id != categoryViewModel.Id) return NotFound();
			if (!ModelState.IsValid) return View(categoryViewModel);

			try
			{
				await _categoryService.UpdateCategoryAsync(categoryViewModel);
				return RedirectToAction(nameof(Index));
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!await _categoryService.CategoryExists(id))
					return NotFound();
				throw;
			}
		} 
		#endregion

		#region Delete
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) return NotFound();

			var category = await _categoryService.GetCategoryByIdAsync(id.Value);
			return category == null ? NotFound() : View(category);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			await _categoryService.DeleteCategoryAsync(id);
			return RedirectToAction(nameof(Index));
		} 
		#endregion 
	}
}