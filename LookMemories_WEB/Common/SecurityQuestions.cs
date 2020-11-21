using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LookMemories_WEB.Common
{
    public class SecurityQuestions
    {
        public int Id { get; set; }
        public string Question { get; set; }

        //LIST of security questions
        public List<SecurityQuestions> GetSecurityQuestion()
        {

            List<SecurityQuestions> question = new List<SecurityQuestions>();
            question.Add(new SecurityQuestions() { Id = 1, Question = "What is Your Fav Animal" });
            question.Add(new SecurityQuestions() { Id = 2, Question = "What is Your Fav Festival" });
            question.Add(new SecurityQuestions() { Id = 3, Question = "What is Your Fav Subject" });
            question.Add(new SecurityQuestions() { Id = 4, Question = "What is Your Fav Food" });
            question.Add(new SecurityQuestions() { Id = 5, Question = "What is Your Fav Country" });

            return question;
        }
    }
}
