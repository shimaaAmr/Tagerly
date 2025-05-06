//using System.Security.Claims;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Tagerly.Models;
//using Tagerly.ViewModels;
//using System.Threading.Tasks;
//using Tagerly.Services.Implementations;

//namespace Tagerly.Controllers
//{

//	public class AccountController : Controller
//	{
//		readonly UserManager<ApplicationUser> _userManager;
//		readonly SignInManager<ApplicationUser> _signInManager;
//		readonly CartService _cartService;

//		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
//		{
//			_userManager = userManager;
//			_signInManager = signInManager;

//		}



//		#region Sign Up
//		[HttpGet]
//		public IActionResult SignUp()
//		{
//			return View();
//		}

//		[HttpPost]
//		[ValidateAntiForgeryToken]
//		public async Task<IActionResult> SignUp(SignUpViewModel userViewModel)
//		{
//			if (!ModelState.IsValid)
//			{
//				return View(userViewModel);
//			}

//			var user = new ApplicationUser
//			{
//				UserName = userViewModel.UserName,
//				Email = userViewModel.Email,
//				PhoneNumber = userViewModel.Phone,
//				Address = userViewModel.Address
//			};

//			var result = await _userManager.CreateAsync(user, userViewModel.Password);
//			if (result.Succeeded)
//			{
//				var resultRole = await _userManager.AddToRoleAsync(user, userViewModel.Role);
//				if (resultRole.Succeeded)
//				{
//					try
//					{
//						await _cartService.CreateNewCartForUser(user.Id);
//					}
//					catch (System.Exception ex)
//					{
//						// Log the error but don't prevent the user from registering
//						ModelState.AddModelError("", "Account created but there was an issue setting up your cart.");
//					}

//					await _signInManager.SignInAsync(user, isPersistent: false);
//					return RedirectToAction("Index", "Home");
//				}

//				foreach (var error in resultRole.Errors)
//				{
//					ModelState.AddModelError("", error.Description);
//				}
//			}

//			foreach (var error in result.Errors)
//			{
//				ModelState.AddModelError("", error.Description);
//			}

//			return View(userViewModel);
//		}
//		#endregion
//		#region Log In
//		[HttpGet]
//		public IActionResult Login()
//		{
//			return View();
//		}


//		[HttpPost]
//		public async Task<IActionResult> Login(LoginViewModel loginViewModel)
//		{
//			if (ModelState.IsValid)
//			{
//				ApplicationUser appUser = await _userManager.FindByEmailAsync(loginViewModel.Email);
//				if (appUser != null)
//				{
//					bool isFound = await _userManager.CheckPasswordAsync(appUser, loginViewModel.Password);
//					if (isFound)
//					{
//						List<Claim> claims = new List<Claim>
//						{
//							new Claim("UserAddress", appUser.Address ?? string.Empty)
//						};
//						await _signInManager.SignInWithClaimsAsync(appUser, loginViewModel.RemmemberMe, claims);
//						return RedirectToAction("Index", "Home");
//					}
//				}
//				ModelState.AddModelError(string.Empty, "Invalid email or password.");
//			}
//			return View(loginViewModel);
//		}
//		#endregion

//		#region Sign Out
//		[HttpGet]
//		public IActionResult Logout()
//		{
//			return View("ConfirmLogout");
//		}

//		[HttpPost]
//		[ValidateAntiForgeryToken]
//		public async Task<IActionResult> ConfirmLogout()
//		{
//			await _signInManager.SignOutAsync();
//			return RedirectToAction(nameof(Login));
//		}
//		#endregion
//	}
//}

using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tagerly.Models;
using Tagerly.ViewModels;
using System.Threading.Tasks;
using Tagerly.Services.Implementations;

namespace Tagerly.Controllers
{
	public class AccountController : Controller
	{
		readonly UserManager<ApplicationUser> _userManager;
		readonly SignInManager<ApplicationUser> _signInManager;
		readonly CartService _cartService;

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
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SignUp(SignUpViewModel userViewModel)
		{
			if (!ModelState.IsValid)
			{
				return View(userViewModel);
			}

			var user = new ApplicationUser
			{
				UserName = userViewModel.UserName,
				Email = userViewModel.Email,
				PhoneNumber = userViewModel.Phone,
				Address = userViewModel.Address
			};

			var result = await _userManager.CreateAsync(user, userViewModel.Password);
			if (result.Succeeded)
			{
				var resultRole = await _userManager.AddToRoleAsync(user, userViewModel.Role);
				if (resultRole.Succeeded)
				{
					try
					{
						await _cartService.CreateNewCartForUser(user.Id);
					}
					catch (System.Exception ex)
					{
						ModelState.AddModelError("", "Account created but there was an issue setting up your cart.");
					}

					List<Claim> claims = new List<Claim>
					{
						new Claim("UserAddress", user.Address ?? string.Empty)
					};
					await _signInManager.SignInWithClaimsAsync(user, isPersistent: false, claims);

					if (userViewModel.Role == "Buyer")
					{
						return RedirectToAction("Index", "Home");
					}
					else if (userViewModel.Role == "Seller")
					{
						return RedirectToAction("Index", "Product");
					}

					return RedirectToAction("Index", "Home");
				}

				foreach (var error in resultRole.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
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
		[ValidateAntiForgeryToken]
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

						// التحقق من دور المستخدم وإعادة توجيهه
						if (await _userManager.IsInRoleAsync(appUser, "Admin"))
						{
							return RedirectToAction("Dashboard", "Admin");
						}
						else if (await _userManager.IsInRoleAsync(appUser, "Buyer"))
						{
							return RedirectToAction("Index", "Home");
						}
						else if (await _userManager.IsInRoleAsync(appUser, "Seller"))
						{
							return RedirectToAction("Index", "Product");
						}
					}
				}
				ModelState.AddModelError(string.Empty, "Email or password not valid");
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