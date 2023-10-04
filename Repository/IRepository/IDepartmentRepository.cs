using AppQuiz.Models;

namespace AppQuiz.Repository.IRepository
{
    public interface IDepartmentRepository: IRepository<Department>
    {
        void Update(Department obj);
        public Department GetDepartmentById(int? departmentId);
    }
}
