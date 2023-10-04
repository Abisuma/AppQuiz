using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AppQuiz.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }
        [DisplayName("Name  of Department")]
        public string? DepartmentalName { get; set; }
        [ValidateNever]
        public List<Question>? Questions { get; set; } = new List<Question>();  


    }
}
