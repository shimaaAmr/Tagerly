using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tagerly.Models;
using Tagerly.ViewModels.AdminViewModel;

namespace Tagerly.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminUserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public AdminUserController(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Users()
        {
            var users = _userManager.Users.ToList();

            var viewModels = new List<UserViewModel>();
            foreach (var user in users)
            {
                var vm = _mapper.Map<UserViewModel>(user);
                var roles = await _userManager.GetRolesAsync(user);
                vm.Role = roles.FirstOrDefault();
                viewModels.Add(vm);
            }

            return View(viewModels);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActivation(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (user.LockoutEnd != null && user.LockoutEnd > DateTime.Now)
            {
                // حاليا مقفول → نفتحه
                user.LockoutEnd = DateTime.Now;
            }
            else
            {
                // حاليا مفتوح → نقفله لمدة 100 سنة مثلًا
                user.LockoutEnd = DateTime.Now.AddYears(100);
            }

            await _userManager.UpdateAsync(user);
            return RedirectToAction("Users");
        }
    }

}
