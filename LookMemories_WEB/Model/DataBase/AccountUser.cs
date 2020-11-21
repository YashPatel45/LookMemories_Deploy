using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LookMemories_WEB.Model.DataBase
{
    public class AccountUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DOB { get; set; }

        public string Gender { get; set; }

        public string AboutMe { get; set; }

        public int SecurityQuestion { get; set; }

        public string Answer { get; set; }

        public List<Album> AlbumList { get; set; }

        [NotMapped]
        public string SecurityQuestionName { get; set; }


    }
}
