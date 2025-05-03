using Gyumri.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace Gyumri.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ArticleController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _env;

        public ArticleController(ApplicationContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Article article, List<IFormFile> imageFiles)
        {
            var filesIndex = 0;

            foreach (var block in article.Blocks)
            {
                if (block.BlockType == "Image")
                {
                    var file = imageFiles[filesIndex++];
                    if (file != null && file.Length > 0)
                    {
                        var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");
                        Directory.CreateDirectory(uploadsPath);

                        var fileName = Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(uploadsPath, fileName);

                        using var stream = new FileStream(filePath, FileMode.Create);
                        await file.CopyToAsync(stream);

                        block.Content = "/uploads/" + fileName;
                    }
                }
            }

            _context.Articles.Add(article);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = article.Id });
        }

        public async Task<IActionResult> Details(int id)
        {
            var article = await _context.Articles
                .Include(a => a.Blocks)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (article == null)
            {
                return NotFound(); // Or redirect to an error page
            }

            return View(article);
        }

    }
}
