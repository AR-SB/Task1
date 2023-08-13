using Microsoft.EntityFrameworkCore;
using Task1.Models;

namespace Task1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Application> Application { get; set; }
        public DbSet<Admin> Admin { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Application>()
                .HasOne<Admin>()
                .WithMany()
                .HasForeignKey(a => a.EmployeeId)
                .IsRequired(false) // Depending on your requirements
                .OnDelete(DeleteBehavior.Restrict); // Adjust this behavior as needed
        }
    }
}
