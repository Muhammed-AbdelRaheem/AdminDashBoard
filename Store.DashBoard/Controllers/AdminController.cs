using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.Core.Dtos.Auth;
using Store.Core.Entities.Identity;

namespace Store.DashBoard.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AdminController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginDto login)
        {


            var user = await _userManager.FindByEmailAsync(login.Email);

            if (user == null)
            {
                ModelState.AddModelError("Email", "Email Is Invalid");
                return RedirectToAction("login");
            }

            var Result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);

            if (Result is null)
            {
                ModelState.AddModelError(string.Empty, "Invalid Login");

            }

            if (!Result.Succeeded /*|| !await _userManager.IsInRoleAsync(user, "admin")*/)
            {
                ModelState.AddModelError(string.Empty, "You Are Not Authorized");
                return RedirectToAction("login");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }


            

             
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }


    }
}
