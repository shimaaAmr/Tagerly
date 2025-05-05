using Tagerly.Models;

namespace Tagerly.Services.Interfaces
{
	public interface IUserService
	{
		Task<bool> IsEmailTakenAsync(string email);
		Task<ApplicationUser> GetUserByIdAsync(string id);
		Task UpdateProfileAsync(UpdateProfileViewModel model);
	}
}
