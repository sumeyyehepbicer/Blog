using Blog.Shared.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Shared.Entities
{
    public class Comment:BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int ArticleId { get; set; }      
        public Article Article { get; set; }
        public string Contents { get; set; }
    }
}
