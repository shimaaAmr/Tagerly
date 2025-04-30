using Microsoft.AspNetCore.Mvc;

namespace Tagerly.Controllers
{
	public class AccountController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
