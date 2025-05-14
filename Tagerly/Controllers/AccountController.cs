
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tagerly.Models;
using System.Threading.Tasks;
using Tagerly.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Tagerly.ViewModels.AccountVieModel;

namespace Tagerly.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IEmailService _emailService;
		private readonly ICartService _cartService;

		public AccountController(
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			IEmailService emailService,
			ICartService cartService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_emailService = emailService;
			_cartService = cartService;
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
					catch (Exception ex)
					{
						ModelState.AddModelError("", "Account created but there was an issue setting up your cart.");
					}

					// Generate token
					var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
					var confirmationLink = Url.Action("ConfirmEmail", "Account",
						new { userId = user.Id, token }, protocol: Request.Scheme);

					// Get email body from email service
					var emailBody = _emailService.GetEmailConfirmationTemplate(user.UserName, confirmationLink);
					await _emailService.SendEmailAsync(user.Email, "Confirm Your Email", emailBody);

					return RedirectToAction(nameof(SignUpSuccess));
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

		[HttpGet]
		public IActionResult SignUpSuccess()
		{
			return View();
		}
		#endregion

		#region Confirmation Email and Resend Confirmation
		[HttpGet]
		public async Task<IActionResult> ConfirmEmail(string userId, string token)
		{
			if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
			{
				return View("Error");
			}

			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				return View("Error");
			}

			var result = await _userManager.ConfirmEmailAsync(user, token);
			if (result.Succeeded)
			{
				List<Claim> claims = new List<Claim>
				{
					new Claim("UserAddress", user.Address ?? string.Empty)
				};
				await _signInManager.SignInWithClaimsAsync(user, isPersistent: false, claims);

				//if (await _userManager.IsInRoleAsync(user, "Buyer"))
				//{
				//	return RedirectToAction("Index", "Home");
				//}
				//else if (await _userManager.IsInRoleAsync(user, "Seller"))
				//{
				//	return RedirectToAction("Index", "Product");
				//}

				//return RedirectToAction("Index", "Home");
				return RedirectToAction("Login", "Account");

			}

			return View("Error");
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult ResendConfirmationEmail()
		{
			return View(new ResendConfirmationEmailViewModel());
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ResendConfirmationEmail(ResendConfirmationEmailViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user != null && !await _userManager.IsEmailConfirmedAsync(user))
			{
				var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
				var confirmationLink = Url.Action("ConfirmEmail", "Account",
					new { userId = user.Id, token }, protocol: Request.Scheme);

				var emailBody = _emailService.GetEmailConfirmationTemplate(user.UserName, confirmationLink);
				await _emailService.SendEmailAsync(model.Email, "Confirm Your Email", emailBody);

				return RedirectToAction("SignUpSuccess");
			}

			return RedirectToAction("SignUpSuccess");
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

				if (appUser == null)
				{
					ModelState.AddModelError(string.Empty, "Email or password not valid");
					return View(loginViewModel);
				}

				if (!appUser.IsActive)
				{
					ModelState.AddModelError(string.Empty, "Your account is deactivated.");
					return View(loginViewModel);
				}

				bool isFound = await _userManager.CheckPasswordAsync(appUser, loginViewModel.Password);
				if (!isFound)
				{
					ModelState.AddModelError(string.Empty, "Email or password not valid");
					return View(loginViewModel);
				}

				if (!appUser.EmailConfirmed)
				{
					ModelState.AddModelError(string.Empty, "Please confirm your email first.");
					return View(loginViewModel);
				}

				List<Claim> claims = new List<Claim>
		{
			new Claim("UserAddress", appUser.Address ?? string.Empty)
		};
				await _signInManager.SignInWithClaimsAsync(appUser, loginViewModel.RemmemberMe, claims);

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

		#region Forgot Password
		[HttpGet]
		[AllowAnonymous]
		public IActionResult ForgotPassword()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user != null && await _userManager.IsEmailConfirmedAsync(user))
				{
					var token = await _userManager.GeneratePasswordResetTokenAsync(user);
					var callbackUrl = Url.Action("ResetPassword", "Account",
						new { email = model.Email, token = token }, Request.Scheme);

					var emailBody = _emailService.GetPasswordResetTemplate(user.UserName, callbackUrl);
					await _emailService.SendEmailAsync(model.Email, "Reset Password", emailBody);

					return RedirectToAction(nameof(ForgotPasswordConfirmation));
				}
				return RedirectToAction(nameof(ForgotPasswordConfirmation));
			}
			return View(model);
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult ResetPassword(string email, string token)
		{
			if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
			{
				return BadRequest("Invalid reset request.");
			}
			var model = new ResetPasswordViewModel { Email = email, Token = token };
			return View(model);
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user == null)
			{
				return RedirectToAction(nameof(ResetPasswordConfirmation));
			}

			var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
			if (result.Succeeded)
			{
				return RedirectToAction(nameof(ResetPasswordConfirmation));
			}

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
			return View(model);
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult ForgotPasswordConfirmation()
		{
			return View();
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult ResetPasswordConfirmation()
		{
			return View();
		}
		#endregion

		public IActionResult AccessDenied()
		{
			return View();
		}
	}
}