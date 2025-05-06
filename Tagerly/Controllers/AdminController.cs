using Microsoft.AspNetCore.Mvc;
using Tagerly.Services.Interfaces.Admin;
using Tagerly.ViewModels.AdminViewModel;

namespace Tagerly.Controllers
{
    public class AdminController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public AdminController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var model = new DashboardViewModel
            {
                TotalUsers = await _dashboardService.GetTotalUsersAsync(),
                TotalProducts = await _dashboardService.GetTotalProductsAsync(),
                TotalOrders = await _dashboardService.GetTotalOrdersAsync(),
                TotalRevenue = await _dashboardService.GetTotalRevenueAsync()
            };

            return View(model);
        }
    }
}
