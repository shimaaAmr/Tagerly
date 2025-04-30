using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tagerly.Models;
using Tagerly.ViewModels;

namespace Tagerly.Controllers
{
	public class AccountController : Controller
	{
		readonly UserManager<ApplicationUser> _userManager;
		readonly SignInManager<ApplicationUser> _signInManager;
		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		#region Sign Up
		[HttpGet]
		public async Task<IActionResult> SignUp()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> SignUp(SignUpViewModel UserViewModel)
		{
			if (ModelState.IsValid)
			{
				//mapping
				ApplicationUser user = new ApplicationUser
				{
					UserName = UserViewModel.UserName,
					Email = UserViewModel.Email,
					PhoneNumber = UserViewModel.Phone,
					Address = UserViewModel.Address,
					PasswordHash = UserViewModel.Password,
				};
				//create user
				IdentityResult result = await _userManager.CreateAsync(user, UserViewModel.Password);
				if (result.Succeeded)
				{
					IdentityResult resultRole = await _userManager.AddToRoleAsync(user, UserViewModel.Role);
					if (resultRole.Succeeded)
					{
						//make cookie
						await _signInManager.SignInAsync(user, isPersistent: false);
						return RedirectToAction("Login", "Account");
					}
					foreach (var error in resultRole.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
				}
				// if error when creeate user
				foreach (var item in result.Errors)
				{
					ModelState.AddModelError("", item.Description);
				}
			}
			return View(UserViewModel);

		}
		#endregion

		#region Log In
		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]

		public async Task<IActionResult> Login(LoginViewModel loginViewModel)
		{
			ApplicationUser appUser = await _userManager.FindByEmailAsync(loginViewModel.Email);
			if (appUser != null)
			{
				bool isFound = await _userManager.CheckPasswordAsync(appUser, loginViewModel.Password);
				if (isFound)
				{
					List<Claim> Claims = new List<Claim>();
					Claims.Add(new Claim("UserAddress", appUser.Address));
					_signInManager.SignInWithClaimsAsync(appUser, loginViewModel.RemmemberMe, Claims);
					return RedirectToAction("Index", "Home");
				}
			}
			ModelState.AddModelError(string.Empty, "Email or Password is incorrect");
			return View(loginViewModel);
		}
		#endregion

		#region Sign Out
		public async Task<IActionResult> SignOut()
		{
			await _signInManager.SignOutAsync();
			return View("Login");
		}
		#endregion


	}

}
