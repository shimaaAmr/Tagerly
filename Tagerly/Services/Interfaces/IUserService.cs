namespace Tagerly.Services.Interfaces
{
	public interface IUserService
	{
		Task<bool> IsEmailTakenAsync(string email);
	}
}
