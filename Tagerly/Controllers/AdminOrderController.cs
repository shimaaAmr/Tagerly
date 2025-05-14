using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tagerly.Services.Interfaces;
using Tagerly.Services.Interfaces.Admin;

namespace Tagerly.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminOrderController : Controller
	{
		private readonly IAdminOrderService _AdminOrderService;

		public AdminOrderController(IAdminOrderService AdminOrderService)
		{
			_AdminOrderService = AdminOrderService;
		}

		public async Task<IActionResult> Index()
		{
			var orders = await _AdminOrderService.GetPendingOrdersAsync();
			return View(orders);
		}

		public async Task<IActionResult> MarkAsDelivered(int id)
		{
			var result = await _AdminOrderService.MarkAsDeliveredAsync(id);
			if (result)
				TempData["Success"] = "Order delivered successfully!";
			else
				TempData["Error"] = "Request not found.";

			return RedirectToAction("Index");
		}
	}
}
