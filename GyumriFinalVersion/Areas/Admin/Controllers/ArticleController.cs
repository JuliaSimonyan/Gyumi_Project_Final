using Gyumri.App.Interfaces;
using Gyumri.App.Services;
using Gyumri.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication20.Models;



namespace Gyumri.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class ArticleController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IArticle _articleService;

        public ArticleController(ApplicationContext context, IWebHostEnvironment env, IArticle articleService)
        {
            _context = context;
            _env = env;
            _articleService = articleService;
        }

        public async Task<IActionResult> Index()
        {
            var articles = await _articleService.GetAllArticles();
            return View(articles);
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
            var article = await _articleService.GetArticleById(id);
            if (article == null)
                return NotFound();

            return View(article);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // Get the article with its blocks
            var article = await _context.Articles
                .Include(a => a.Blocks)  // Ensure the blocks are included
                .FirstOrDefaultAsync(a => a.Id == id);

            // Check if article was found
            if (article == null)
            {
                return NotFound();
            }

            // Log the article and blocks for debugging
            Console.WriteLine($"Article ID: {article.Id}, Title: {article.Title}");
            foreach (var block in article.Blocks)
            {
                Console.WriteLine($"Block Type: {block.BlockType}, Content: {block.Content}, Order: {block.Order}");
            }

            return View(article);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Article model, List<IFormFile> imageFiles)
        {
            var existing = await _context.Articles
                .Include(a => a.Blocks)
                .FirstOrDefaultAsync(a => a.Id == model.Id);

            if (existing == null)
                return NotFound();

            // Update the article title
            existing.Title = model.Title;

            int imageIndex = 0;

            for (int i = 0; i < model.Blocks.Count; i++)
            {
                var newBlock = model.Blocks[i];
                var existingBlock = existing.Blocks.FirstOrDefault(b => b.Id == newBlock.Id);

                if (existingBlock != null)
                {
                    // Update the text block content
                    if (existingBlock.BlockType == "Text")
                    {
                        existingBlock.Content = newBlock.Content;  // Here we save the edited paragraph content
                    }
                    // Update the image block content
                    else if (existingBlock.BlockType == "Image")
                    {
                        if (imageFiles.Count > imageIndex && imageFiles[imageIndex]?.Length > 0)
                        {
                            var file = imageFiles[imageIndex++];
                            var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");
                            Directory.CreateDirectory(uploadsPath);
                            var fileName = Path.GetFileName(file.FileName);
                            var filePath = Path.Combine(uploadsPath, fileName);

                            using var stream = new FileStream(filePath, FileMode.Create);
                            await file.CopyToAsync(stream);

                            existingBlock.Content = "/uploads/" + fileName;
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var article = await _context.Articles
                .Include(a => a.Blocks)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (article == null)
                return NotFound();

            _context.ArticleBlocks.RemoveRange(article.Blocks); 
            _context.Articles.Remove(article);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


    }
}
