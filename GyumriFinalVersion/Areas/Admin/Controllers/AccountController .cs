using Gyumri.Common.ViewModel;
using GyumriFinalVersion.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GyumriFinalVersion.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            Console.WriteLine($"Attempting login for: {model.Email}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model is invalid");
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            Console.WriteLine(user == null ? "User not found." : "User found.");

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            Console.WriteLine($"Login succeeded: {result.Succeeded}");

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            Console.WriteLine("Login failed.");
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }
       
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied() => View();

    }
}
