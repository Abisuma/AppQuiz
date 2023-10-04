using AppQuiz.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppQuiz.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser> 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        
        public DbSet<Department> Departments { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<UserResult> UserResults { get; set; }
        
        public DbSet<Question> Questions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
