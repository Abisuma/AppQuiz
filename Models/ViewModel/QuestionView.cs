using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using AppQuiz.Models;

namespace AppQuiz.Models.ViewModel
{
    public class QuestionView
    {
        public Question? Question { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? DepartmentList { get; set; }
    }
}
