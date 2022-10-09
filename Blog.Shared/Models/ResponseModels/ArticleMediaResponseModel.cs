using Blog.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Shared.Models.ResponseModels
{
    public class ArticleMediaResponseModel
    {
        public Article Article { get; set; }
        public FileMedia FileMedia { get; set; }
    }
}
