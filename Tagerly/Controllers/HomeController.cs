//using System.Diagnostics;
//using Microsoft.AspNetCore.Mvc;
//using Tagerly.DataAccess.DbContexts;
//using Tagerly.Models;

//namespace Tagerly.Controllers
//{
//    public class HomeController : Controller
//    {
//        private readonly ILogger<HomeController> _logger;

//        public HomeController(ILogger<HomeController> logger)
//        {
//            _logger = logger;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

//        public IActionResult Privacy()
//        {
//            return View();
//        }

//        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//        public IActionResult Error()
//        {
//            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//        }
//    }
//}
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Tagerly.Models;
using System.Linq;
using Tagerly.DataAccess.DbContexts;

namespace Tagerly.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TagerlyDbContext _context;

        public HomeController(
            ILogger<HomeController> logger,
            TagerlyDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            ViewBag.Categories = categories;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
