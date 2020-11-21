using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LookMemories_WEB.Model.ViewModel
{
    //VIEWMODEL for edit user profile
    public class EidtUserProfileViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        [StringLength(25)]
        public string FName { get; set; }

        [StringLength(25)]
        public string LName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Gender { get; set; }

        public string AboutMe { get; set; }

        public string DOB { get; set; }
    }
}
