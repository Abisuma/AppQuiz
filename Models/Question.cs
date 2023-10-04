using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppQuiz.Models
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public string AnswerA { get; set; }
        public string AnswerB { get; set; }
        public string AnswerC { get; set; }
        public string AnswerD { get; set; }

        public string? CorrectAnswer { get; set; }

        [ForeignKey("DepartmentId")]
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

    }
}
