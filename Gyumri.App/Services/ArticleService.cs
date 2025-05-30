using Gyumri.App.Interfaces;
using Gyumri.Data.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication20.Models;


namespace Gyumri.App.Services
{
    public class ArticleService : IArticle
    {
        private readonly ApplicationContext _context;

        public ArticleService(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<List<Article>> GetAllArticles()
        {
            return await _context.Articles
           .Include(a => a.Blocks) 
           .Select(a => new Article
           {
               Id = a.Id,
               Title = a.Title,
               TitleArm = a.TitleArm,
               TitleRus = a.TitleRus,
               Blocks = a.Blocks.Select(b => new ArticleBlock
               {
                   Id = b.Id,
                   BlockType = b.BlockType,
                   Content = b.Content,
                   ContentArm = b.ContentArm,
                   ContentRus = b.ContentRus,
                   Order = b.Order
               }).ToList()
           })
           .ToListAsync();
        }

        public async Task<Article> GetArticleById(int id)
        {
            var article = await _context.Articles
                .Include(a => a.Blocks) 
                .FirstOrDefaultAsync(a => a.Id == id);

            if (article == null)
                return null;

            return new Article
            {
                Id = article.Id,
                Title = article.Title,
                TitleArm = article.TitleArm,
                TitleRus = article.TitleRus,
                Blocks = article.Blocks.Select(b => new ArticleBlock
                {
                    Id = b.Id,
                    BlockType = b.BlockType,
                    Content = b.Content,
                    ContentArm = b.ContentArm,
                    ContentRus = b.ContentRus,
                    Order = b.Order
                }).ToList()
            };
        }
    }
   
}
