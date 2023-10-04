namespace AppQuiz.Repository.IRepository
{
    public interface IUnitOfWork
    {
        
            IDepartmentRepository Department { get; }
            
            IQuestionRepository Question { get; }

            IUserResultRepository UserResult { get; }
            void Save();


       
    }
}
