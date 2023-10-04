using AppQuiz.Data;
using AppQuiz.Models;
using AppQuiz.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using static AppQuiz.Repository.DepartmentRepository;

namespace AppQuiz.Repository
{
    public class UserResultRepository : Repository<UserResult>, IUserResultRepository
    {
       
            private AppDbContext _dbContext;
            public UserResultRepository(AppDbContext DbContext) : base(DbContext)
            {
                _dbContext = DbContext;
            }



            public void Update(UserResult obj)
            {
                _dbContext.UserResults.Update(obj);
            }
        public UserResult GetApplicationUserResult(int id)
        {
            return _dbContext.UserResults.Include(d => d.ApplicationUser).FirstOrDefault(d => d.Id == id);
        }
    }
}
