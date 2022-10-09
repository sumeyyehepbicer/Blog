using Blog.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Shared.Entities
{
    public class LikeIt:BaseEntity
    {
        public int UserId { get; set; }
        public int ArticleId { get; set; }
    }
}
