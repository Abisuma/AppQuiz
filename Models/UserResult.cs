using System.ComponentModel.DataAnnotations.Schema;

namespace AppQuiz.Models
{
    public class UserResult
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        
        public double Score { get; set; }


    }
}
