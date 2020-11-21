using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LookMemories_WEB.Model.ViewModel
{
    //VIEWMODEL for Login
    public class LoginViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsRemember { get; set; }
    }
}
