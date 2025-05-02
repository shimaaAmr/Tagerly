using Microsoft.EntityFrameworkCore;
using Tagerly.DataAccess;
using Tagerly.Services.Interfaces;

namespace Tagerly.Services
{
	public class UserService : IUserService
	{
		private readonly TagerlyDbContext _context;

		public UserService(TagerlyDbContext context)
		{
			_context = context;
		}

		public async Task<bool> IsEmailTakenAsync(string email)
		{
			return await _context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
		}

	}
}