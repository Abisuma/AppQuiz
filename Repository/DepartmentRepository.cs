using AppQuiz.Data;
using AppQuiz.Models;
using AppQuiz.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using static AppQuiz.Repository.DepartmentRepository;

namespace AppQuiz.Repository
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
       
            private AppDbContext _dbContext;
            public DepartmentRepository(AppDbContext DbContext) : base(DbContext)
            {
                _dbContext = DbContext;
            }



            public void Update(Department obj)
            {
                _dbContext.Departments.Update(obj);
            }


        public Department GetDepartmentById(int? departmentId)
        {
            return _dbContext.Departments.Include(d => d.Questions).FirstOrDefault(d => d.DepartmentId == departmentId);
        }
    }
}
