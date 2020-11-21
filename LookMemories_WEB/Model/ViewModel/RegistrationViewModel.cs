using LookMemories_WEB.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LookMemories_WEB.Model.ViewModel
{
    //VIEWMODEL for Regidytration
    public class RegistrationViewModel
    {
        public string UserName { get; set; }

        [StringLength(25)]
        public string FName { get; set; }

        [StringLength(25)]
        public string LName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]

        public string ConfirmPassword { get; set; }

        public int QuestionId { get; set; }

        [StringLength(6,ErrorMessage = "Answer Should be in 6 Characters")]
        public string Answer { get; set; }

        public List<SecurityQuestions> SecurityQuestionsList { get; set; }
    }
}
