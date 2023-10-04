using AppQuiz.Models;

namespace AppQuiz.Repository.IRepository
{
    public interface IQuestionRepository: IRepository<Question>
    {
        void Update(Question obj);
        List<Question> GetQuestionsForDepartment(int departmentId);
    }
}
