//using Microsoft.EntityFrameworkCore;
//using Tagerly.DataAccess;
//using Tagerly.Models;
//using Tagerly.Services.Interfaces;

//namespace Tagerly.Services
//{
//	public class UserService : IUserService
//	{
//		private readonly TagerlyDbContext _context;

//		public UserService(TagerlyDbContext context)
//		{
//			_context = context;
//		}

//		public async Task<bool> IsEmailTakenAsync(string email)
//		{
//			return await _context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
//		}


//	}
//}
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;
using Tagerly.DataAccess;
using Tagerly.Models;
using Tagerly.Services.Interfaces;

namespace Tagerly.Services
{
	public class UserService : IUserService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly TagerlyDbContext _context;

		public UserService(UserManager<ApplicationUser> userManager, TagerlyDbContext context)
		{
			_userManager = userManager;
			_context = context;
		}
		public async Task<bool> IsEmailTakenAsync(string email)
		{
			return await _context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
		}
		public async Task<ApplicationUser> GetUserByIdAsync(string id)
		{
			return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
		}

		public async Task UpdateProfileAsync(UpdateProfileViewModel model)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.Id);
			if (user == null)
			{
				throw new Exception("User not found.");
			}

			// تحديث بيانات المستخدم
			user.UserName = model.UserName;
			user.Email = model.Email;
			user.Address = model.Address;
			user.PhoneNumber = model.PhoneNumber;

			// التحقق من رفع الصورة
			if (model.ProfilePicture != null && model.ProfilePicture.Length > 0)
			{
				// تحديد مسار حفظ الصورة
				var fileName = Path.GetFileNameWithoutExtension(model.ProfilePicture.FileName);
				var extension = Path.GetExtension(model.ProfilePicture.FileName);
				var newFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
				var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profiles", newFileName);

				// التأكد من وجود المجلد
				var directory = Path.GetDirectoryName(filePath);
				if (!Directory.Exists(directory))
				{
					Directory.CreateDirectory(directory);
				}

				// حفظ الصورة
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await model.ProfilePicture.CopyToAsync(stream);
				}

				// تحديث مسار الصورة في قاعدة البيانات
				user.ProfilePicturePath = $"/images/profiles/{newFileName}";
			}

			// حفظ التغييرات
			await _context.SaveChangesAsync();
		}
	}
}