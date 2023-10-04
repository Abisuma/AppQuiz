using AppQuiz.Data;
using AppQuiz.Repository.IRepository;

namespace AppQuiz.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        
            private AppDbContext _dbContext;
            public IDepartmentRepository Department { get; private set; }
           
            public IQuestionRepository Question { get; private set; }

            public IUserResultRepository UserResult { get; private set; }

        public UnitOfWork(AppDbContext DbContext)
        {
                    _dbContext = DbContext;
                    Department = new DepartmentRepository(_dbContext);
                    Question = new QuestionRepository(_dbContext);
                    UserResult = new UserResultRepository(_dbContext);
        }
        public void Save()
            {
                _dbContext.SaveChanges();
            }
        
    }
}
