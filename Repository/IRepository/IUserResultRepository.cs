using AppQuiz.Models;

namespace AppQuiz.Repository.IRepository
{
    public interface IUserResultRepository: IRepository<UserResult>
    {
        void Update(UserResult obj);
        public UserResult GetApplicationUserResult(int id);
    }
}
