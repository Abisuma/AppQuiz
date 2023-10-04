using AppQuiz.Models;
using AppQuiz.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppQuiz.Controllers
{
    [Authorize(Roles = SD.Role_Users)]
    public class UserResultController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserResultController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; 
        }
        public IActionResult Index(int id)
        {
            var userResult = _unitOfWork.UserResult.GetApplicationUserResult(id);
        

            if (userResult != null)
              {
                    return View(userResult);
              }
              else
              {
                return View("Error");
              }
     
        }
    }
}
