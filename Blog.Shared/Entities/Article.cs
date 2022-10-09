using Blog.Shared.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Shared.Entities
{
    public class Article:BaseEntity
    {        
        public int UserId { get; set; }
        public User User { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(200)]
        public string Summary { get; set; }

        [MinLength(200)]
        public string Description { get; set; }
    }
}
