using Gyumri.Common.ViewModel;
using GyumriFinalVersion.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

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
        [HttpGet]
        public IActionResult UploadImage()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(uploadsFolder);

                var originalFileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                var fileExt = Path.GetExtension(imageFile.FileName);
                var optimizedName = $"{originalFileName}_{DateTime.UtcNow.Ticks}.webp";

                var savePath = Path.Combine(uploadsFolder, optimizedName);

                using var stream = imageFile.OpenReadStream();
                using var image = await Image.LoadAsync(stream);

                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(1024, 768),
                    Mode = ResizeMode.Max
                }));

                var encoder = new SixLabors.ImageSharp.Formats.Webp.WebpEncoder
                {
                    Quality = 75
                };

                await image.SaveAsync(savePath, encoder);

                ViewBag.UploadedImage = $"/uploads/{optimizedName}";
                ViewBag.Message = $"Image '{optimizedName}' uploaded and optimized.";
            }

            return View();
        }


    }
}
