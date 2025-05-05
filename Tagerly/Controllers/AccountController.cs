
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tagerly.Models;
using Tagerly.ViewModels;
using System.Threading.Tasks;

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
		public IActionResult SignUp()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SignUp(SignUpViewModel userViewModel)
		{
			if (ModelState.IsValid)
			{
				ApplicationUser user = new ApplicationUser
				{
					UserName = userViewModel.UserName,
					Email = userViewModel.Email,
					PhoneNumber = userViewModel.Phone,
					Address = userViewModel.Address,
				};
				IdentityResult result = await _userManager.CreateAsync(user, userViewModel.Password);
				if (result.Succeeded)
				{
					IdentityResult resultRole = await _userManager.AddToRoleAsync(user, userViewModel.Role);
					if (resultRole.Succeeded)
					{
						// Sign in the user directly after successful registration
						await _signInManager.SignInAsync(user, isPersistent: false);
						return RedirectToAction("Index", "Home"); // Redirect to Index after successful sign-up
					}
					foreach (var error in resultRole.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
				}
				// If error when creating user
				foreach (var item in result.Errors)
				{
					ModelState.AddModelError("", item.Description);
				}
			}
			return View(userViewModel);
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
			if (ModelState.IsValid)
			{
				ApplicationUser appUser = await _userManager.FindByEmailAsync(loginViewModel.Email);
				if (appUser != null)
				{
					bool isFound = await _userManager.CheckPasswordAsync(appUser, loginViewModel.Password);
					if (isFound)
					{
						List<Claim> claims = new List<Claim>
						{
							new Claim("UserAddress", appUser.Address ?? string.Empty)
						};
						await _signInManager.SignInWithClaimsAsync(appUser, loginViewModel.RemmemberMe, claims);
						return RedirectToAction("Index", "Home");
					}
				}
				ModelState.AddModelError(string.Empty, "Invalid email or password.");
			}
			return View(loginViewModel);
		}
		#endregion

		#region Sign Out
		[HttpGet]
		public IActionResult Logout()
		{
			return View("ConfirmLogout");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ConfirmLogout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(Login));
		}
		#endregion
	}
}
