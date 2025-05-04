using Gyumri.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyumri.App.Interfaces
{
    public interface IArticle
    {
        Task<List<Article>> GetAllArticles();
        Task<Article> GetArticleById(int id);
    }
}
