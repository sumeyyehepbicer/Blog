using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Shared.Models.ResponseModels
{
    public class AuthResponseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }       
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public bool Gender { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
