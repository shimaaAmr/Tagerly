using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tagerly.Models;
using Tagerly.ViewModels;

namespace Tagerly.Controllers
{
	public class RoleController : Controller
	{
		readonly RoleManager<IdentityRole> _roleManager;
		public RoleController(RoleManager<IdentityRole> roleManager)
		{
			_roleManager = roleManager;
		}
		#region Add Role
		[HttpGet]
		public IActionResult AddRole()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> AddRole(RoleViewModel roleViewModel)
		{
			if (ModelState.IsValid)
			{
				//mapping
				IdentityRole role = new IdentityRole()
				{
					Name = roleViewModel.RoleName
				};
				//Save to DB
				IdentityResult result = await _roleManager.CreateAsync(role);
				if (result.Succeeded)
				{
					return RedirectToAction("Index", "Home");
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}
			return View(roleViewModel);
		}
		#endregion

		#region Delete Role
		[HttpGet]
		public async Task<IActionResult> DeleteRole()
		{
			var roles = _roleManager.Roles.Select(r => r.Name).ToList();
			ViewBag.Roles = new SelectList(roles);
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> DeleteRole(string selectedRole)
		{
			if (string.IsNullOrEmpty(selectedRole))
			{
				ModelState.AddModelError("", "Please select a role.");
			}
			else
			{
				var role = await _roleManager.FindByNameAsync(selectedRole);
				if (role != null)
				{
					var result = await _roleManager.DeleteAsync(role);
					if (result.Succeeded)
					{
						ViewBag.Message = "Role deleted successfully.";
						return RedirectToAction("DeleteRole");
					}
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
				}
				else
				{
					ModelState.AddModelError("", "Role not found.");
				}
			}

			var roles = _roleManager.Roles.Select(r => r.Name).ToList();
			ViewBag.Roles = new SelectList(roles);
			return View();
		}

		#endregion



	}
}
