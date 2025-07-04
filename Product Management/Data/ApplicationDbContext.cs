using Microsoft.EntityFrameworkCore;
using Product_Management.Models;

namespace Product_Management.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                    : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
                .Property(c => c.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Product>()
                .Property(p => p.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
