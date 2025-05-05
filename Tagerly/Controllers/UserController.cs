
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tagerly.Models;
using System.IO;
using System.Threading.Tasks;
using Tagerly.DataAccess.DbContexts;
using Tagerly.Services.Interfaces;

namespace Tagerly.Controllers
{
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _identityUserManager;
		private readonly TagerlyDbContext _context;
		private readonly IUserService _profileService;

		public UserController(UserManager<ApplicationUser> identityUserManager, TagerlyDbContext context, IUserService profileService)
		{
			_identityUserManager = identityUserManager;
			_context = context;
			_profileService = profileService;
		}

		[HttpGet]
		public async Task<IActionResult> UpdateProfile(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				var currentUser = await _identityUserManager.GetUserAsync(User);
				if (currentUser == null)
				{
					return NotFound("User not found.");
				}
				id = currentUser.Id;
			}

			var targetUser = await _profileService.GetUserByIdAsync(id);
			if (targetUser == null)
			{
				return NotFound();
			}

			var model = new UpdateProfileViewModel
			{
				Id = targetUser.Id,
				UserName = targetUser.UserName,
				Email = targetUser.Email,
				Address = targetUser.Address,
				PhoneNumber = targetUser.PhoneNumber,
				ProfilePicturePath = targetUser.ProfilePicturePath
			};

			ViewData["UserId"] = targetUser.Id;
			ViewData["UserName"] = targetUser.UserName;
			ViewData["Email"] = targetUser.Email;
			ViewData["Country"] = targetUser.Address?.Split(',').Last().Trim() ?? "United States";
			ViewData["ProfilePicturePath"] = targetUser.ProfilePicturePath;

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateProfile(UpdateProfileViewModel model)
		{
			if (ModelState.IsValid)
			{
				if (model.ProfilePicture != null)
				{
					var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profiles");
					if (!Directory.Exists(uploadsDir))
					{
						Directory.CreateDirectory(uploadsDir);
					}

					var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(model.ProfilePicture.FileName)}";
					var filePath = Path.Combine(uploadsDir, fileName);
					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await model.ProfilePicture.CopyToAsync(stream);
					}

					model.ProfilePicturePath = $"/images/profiles/{fileName}";
				}


				await _profileService.UpdateProfileAsync(model);

				var updatedUser = await _profileService.GetUserByIdAsync(model.Id);
				ViewData["UserId"] = updatedUser.Id;
				ViewData["UserName"] = updatedUser.UserName;
				ViewData["Email"] = updatedUser.Email;
				ViewData["Country"] = updatedUser.Address?.Split(',').Last().Trim() ?? "United States";
				ViewData["ProfilePicturePath"] = updatedUser.ProfilePicturePath;

				return RedirectToAction("UpdateProfile", new { id = model.Id });
			}
			ViewData["UserId"] = model.Id;
			ViewData["UserName"] = model.UserName;
			ViewData["Email"] = model.Email;
			ViewData["Country"] = model.Address?.Split(',').Last().Trim() ?? "United States";
			ViewData["ProfilePicturePath"] = model.ProfilePicturePath;

			return View(model);
		}
	}
}