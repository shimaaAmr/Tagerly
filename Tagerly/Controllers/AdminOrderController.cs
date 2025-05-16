using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> Index(string status = "", string search = "")
        {
            var orders = await _AdminOrderService.GetFilteredOrdersAsync(status, search);
            ViewBag.StatusFilter = status;
            ViewBag.SearchQuery = search;
            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsDelivered(int id)
        {
            var result = await _AdminOrderService.MarkAsDeliveredAsync(id);
            if (result)
                TempData["Success"] = "Order delivered successfully!";
            else
                TempData["Error"] = "Request not found.";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> OrderDetails(int id)
        {
            var orderDetailsVM = await _AdminOrderService.GetOrderDetailsViewModelByIdAsync(id);

            if (orderDetailsVM == null)
                return NotFound();

            return View(orderDetailsVM);
        }
    }
}
