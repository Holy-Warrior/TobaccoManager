using Microsoft.EntityFrameworkCore;
using TobaccoManager.Models;

namespace TobaccoManager.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Bundle> Bundles { get; set; }
        public DbSet<QuotaAgreement> QuotaAgreements { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=tobacco_manager.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stock>()
                .HasMany(s => s.Bundles)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Bundle>()
                .Property(b => b.LeafGrade)
                .HasConversion<string>();
                
            modelBuilder.Entity<QuotaAgreement>()
                .HasOne(q => q.Customer)
                .WithMany(c => c.QuotaAgreements)
                .HasForeignKey(q => q.CustomerId)
                .OnDelete(DeleteBehavior.Cascade); // Delete agreements when customer is deleted
        }
    }
}