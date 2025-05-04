using Gyumri.Data.Models;

namespace WebApplication20.Models
{
    public class ArticleBlock
    {
        public int Id { get; set; } 
        public string BlockType { get; set; } 
        public string Content { get; set; }
        public int Order { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
    }

}
