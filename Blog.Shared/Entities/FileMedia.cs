using Blog.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Shared.Entities
{
    public class FileMedia:BaseEntity
    {
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public string MediaUrl { get; set; }
        public bool IsCover { get; set; }
    }
}
