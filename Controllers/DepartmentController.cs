using AppQuiz.Models.ViewModel;
using AppQuiz.Models;
using AppQuiz.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using AppQuiz.Data;

namespace AppQuiz.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly AppDbContext appDb;
        public DepartmentController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager/*, AppDbContext con*/)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
           // appDb = con;
        }

        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Index()
        {
            
            List<Department> departmentList = _unitOfWork.Department.GetAll().ToList();
            return View(departmentList);
        }
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult CreateDept()
        {
            DepartmentView vm = new()
            {
                Department = new Department()
            };

            return View(vm);
        }

        [Authorize(Roles = SD.Role_Admin)]
        [HttpPost]
        public IActionResult CreateDept(DepartmentView obj)
        {
            if (ModelState.IsValid)
            {

                _unitOfWork.Department.AddAObj(obj.Department);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View();
        }

        
        [Authorize(Roles = SD.Role_Users)]
        public IActionResult GoToQuiz()
        {
            var departmentId = TempData["SelectedDepartmentId"] as int?;
            if (departmentId != null)
            {
                // Use departmentId for your logic
                var department = _unitOfWork.Department.GetDepartmentById(departmentId);
               
                DepartmentView vm = new()
                {
                    Department = department   
                };

                return View(vm);
            }

            return View();
        }

       [Authorize(Roles = SD.Role_Users)]
        [HttpPost]
        public IActionResult GoToQuiz(DepartmentView obj)
        {
            if (ModelState.IsValid)
            {
                
                var departmentId = obj.Department.DepartmentId;
                var questions = _unitOfWork.Question.GetQuestionsForDepartment(departmentId);

                if(questions == null) { return View("Error"); }
                obj.Department.Questions = questions;   
                
                double score = CalculateScore(obj.Department.Questions);

                ApplicationUser currentUser = _userManager.GetUserAsync(User).Result;

                // Create a UserResult object to store the result in the database.
                UserResult userResult = new UserResult
                {
                    ApplicationUserId = currentUser.Id,
                    ApplicationUser = currentUser ,  
                    Score = Math.Round((score / questions.Count) *100,0 )
                    
                };

                // Add the user's result to the database.
                _unitOfWork.UserResult.AddAObj(userResult);
                _unitOfWork.Save();

                return RedirectToAction("Index", "UserResult", new {userResult.Id});
            }

            // If ModelState is not valid, return the view with validation errors.
            return View(obj);
        }

        // Helper method to calculate the user's score based on their answers.
        [NonAction]
        private double CalculateScore(List<Question> questions)
        {
          double score = 0;

            for (int i = 0; i < questions.Count; i++)
            {
                var question = questions[i];
                var namePrefix = $"answer_{i}";
                var selectedAnswer = Request.Form[namePrefix];
                var correctAnswer = GetFullAnswerText(question, question.CorrectAnswer);

                if (selectedAnswer == correctAnswer)
                {
                    score++;
                }
            }

            return score;
        }


                
        [NonAction]
        private string GetFullAnswerText(Question question, string letterRepresentation)
        {
            // Implement a method to map letter representation to full answer text
            switch (letterRepresentation)
            {
                case "A":
                    return question.AnswerA;
                case "B":
                    return question.AnswerB;
                case "C":
                    return question.AnswerC;
                case "D":
                    return question.AnswerD;
                default:
                    return string.Empty; // Handle unknown representations
            }
        }

    }
}

