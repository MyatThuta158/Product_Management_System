using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Product_Management.Models;

namespace Product_Management.Data
{
    public class ApplicationDbContext:IdentityDbContext<BaseUserClass,IdentityRole,string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                    : base(options) { }


        public DbSet<Customer> Customers { get; set; }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BaseUserClass>().ToTable("AspNetUsers");

            // Now map each subclass to its own TPT table
            modelBuilder.Entity<Admin>()
                   .ToTable("Admins");
            modelBuilder.Entity<Customer>()
                   .ToTable("Customers");

            modelBuilder.Entity<BaseUserClass>().Ignore(u => u.NormalizedUserName);
            modelBuilder.Entity<BaseUserClass>().Ignore(u => u.UserName);
            modelBuilder.Entity<Category>()
                .Property(c => c.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Product>()
                .Property(p => p.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
