﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication20.Models;

namespace Gyumri.Data.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? TitleArm { get; set; }
        public string? TitleRus { get; set; }
        public List<ArticleBlock>? Blocks { get; set; }
    }
}
