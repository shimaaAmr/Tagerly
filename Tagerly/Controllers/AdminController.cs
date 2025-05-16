using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tagerly.Services.Interfaces.Admin;
using Tagerly.ViewModels.AdminViewModel;

namespace Tagerly.Controllers
{
    [Authorize(Roles = "Admin")] 
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
                TotalRevenue = await _dashboardService.GetTotalRevenueAsync(),
                TopSellingProducts = await _dashboardService.GetTopSellingProductsAsync(5) // مثلاً Top 5
            };

            return View(model);
        }
    }
}
