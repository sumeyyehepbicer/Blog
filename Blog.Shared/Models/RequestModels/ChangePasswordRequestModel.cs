using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Shared.Models.RequestModels
{
    public class ChangePasswordRequestModel
    {
        public string Email { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string TryNewPassword { get; set; }
    }
}
