using Microsoft.EntityFrameworkCore;
using AppQuiz.Data;
using AppQuiz.Repository.IRepository;
using System.Linq.Expressions;

namespace AppQuiz.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext db;

        internal DbSet<T> dbSet;
        public Repository(AppDbContext DbContext)
        {
            db = DbContext;
            this.dbSet = db.Set<T>();
            db.Questions.Include(u => u.Department);
            db.Departments.Include(u => u.Questions); 
            db.UserResults.Include(u =>u.ApplicationUser);

        }
        public void AddAObj(T obj)
        {
            dbSet.Add(obj);
        }

        public T GetAObj(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))//check if null or not
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))//it comes as a series of strings with comma seperating them. 
                {
                    query = query.Include(includeProp);
                }
            }

            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if (!string.IsNullOrEmpty(includeProperties))//check if null or not
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))//it comes as a series of strings with comma seperating them. 
                {
                    query = query.Include(includeProp);
                }
            }

            return query.ToList();
        }

        public void RemoveAObj(T obj)
        {
            dbSet.Remove(obj);
        }
    }
        
}
