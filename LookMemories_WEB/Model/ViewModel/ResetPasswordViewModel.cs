using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LookMemories_WEB.Model.ViewModel
{
    //VIEWMODEL for Rest Password
    public class ResetPasswordViewModel
    {
        public string UserId { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }

        public string OldPassword { get; set; }

        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]

        public string ConfirmPassword { get; set; }
    }
}
