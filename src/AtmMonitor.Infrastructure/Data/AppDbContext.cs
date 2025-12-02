using AtmMonitor.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace AtmMonitor.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options) { }
        
        public DbSet<ATM> ATMs { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ATM>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Address).IsRequired().HasMaxLength(500);
                entity.Property(a => a.InstallationDate).IsRequired();
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.OperationDate).IsRequired();
                entity.Property(t => t.Type).IsRequired();
                entity.Property(t => t.Amount).IsRequired();

                entity.HasOne(t => t.ATM)
                      .WithMany(a => a.Transactions)
                      .HasForeignKey(t => t.ATMId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
