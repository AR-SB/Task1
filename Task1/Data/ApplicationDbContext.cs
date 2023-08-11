using Task1.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace Task1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Application> Application { get; set; }
        public DbSet<Admin> Admin { get; set; }

    }
}
  
   

