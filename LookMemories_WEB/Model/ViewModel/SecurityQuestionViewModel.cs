using LookMemories_WEB.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LookMemories_WEB.Model.ViewModel
{
    //VIEWMODEL for Security Questions
    public class SecurityQuestionViewModel
    {
        public string UserId { get; set; }

        public string Email { get; set; }

        public int QuestionId { get; set; }

        public string Answer { get; set; }

        public List<SecurityQuestions> SecurityQuestionsList { get; set; }
    }
}
