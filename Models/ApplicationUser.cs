using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppQuiz.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(20, MinimumLength = 4)]
        [DisplayName("User Name")]
        public string? NameOfUser { get; set; }
        [Required]
        public int DepartmentId { get; set; }
 
        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }
        
        [Required]
        public string? ImageUrl { get; set; }
    }
}
