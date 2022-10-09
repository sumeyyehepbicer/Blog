using Blog.Shared.Common;
using Blog.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Shared.Entities
{
    public class User:BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        [MaxLength(10)]
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public bool Gender { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        //profileImg Eklenecek
    }
}
